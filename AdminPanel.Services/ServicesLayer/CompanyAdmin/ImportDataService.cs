using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared;
using AdminPanel.Shared.BO;
using AutoMapper;
using EFCore.BulkExtensions.SqlAdapters;
using System.Text.Json;
using System.Text.Json.Serialization;
using AdminPanel.DataModel.Context;
using EZOrders.ImportServices;
using MySqlConnector;
using AdminPanel.Services.Helpers;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace AdminPanel.Services.ServicesLayer.CompanyAdmin
{
    public class ImportDataService : IimportDataService
    {
        private readonly IGenericRepository<ProductImport> _product;
        private readonly IGenericRepository<CustomerImport> _customer;
        private readonly IGenericRepository<SpecialPriceImport> _specialPriceImport;

        private readonly IGenericRepository<CompanyDataImportMapping> _companyDataImportMappings;
        private readonly IGenericRepository<ImportLog> _importlog;
        private readonly IMapper _mapper;
        public readonly ImportHelper _importHelper;
        private readonly OrderflowContext _context;

        public ImportDataService(IGenericRepository<ProductImport> product,
            IGenericRepository<CompanyDataImportMapping> companyDataImportMappings,
            IGenericRepository<ImportLog> importlogs,
            IGenericRepository<CustomerImport> customer,
                            IMapper mapper, IConfiguration config, IGenericRepository<SpecialPriceImport> specialPriceImport, OrderflowContext context)
        {
            _product = product;
            _companyDataImportMappings = companyDataImportMappings;
            _importlog = importlogs;
            _customer = customer;
            _mapper = mapper;
            _importHelper = new ImportHelper(config);
            _specialPriceImport = specialPriceImport;
            _context = context;
        }

        public IEnumerable<CommonDropdownBO> GetImportedMapping(int companyId, string Importfor)
        {
            var result = _companyDataImportMappings.GetWithConditions(x => x.CompanyId == companyId && x.ImportType == Importfor).ToList();
            return _mapper.Map<IEnumerable<CommonDropdownBO>>(result);

        }

        public IEnumerable<FieldInfo> GetImportedTableColumns(ImportFor importType)
        {
            IEnumerable<FieldInfo> enumData = null;
            System.Reflection.PropertyInfo[] columnsArray = null;
            List<string> RequiredFields = new List<string>();
            if (importType == ImportFor.Product)
            {
                columnsArray = typeof(ProductImport).GetProperties();

                string[] arra = { "Name", "Code", "CategoryName", "MainCategoryName", "UnitId", "Price", "FilterName" };
                RequiredFields = arra.ToList();
            }
            if (importType == ImportFor.Customer)
            {
                columnsArray = typeof(CustomerImport).GetProperties();

                string[] arra = { "AlphaCode", "CustomerName", "Email" };
                RequiredFields = arra.ToList();


            }
            if (importType == ImportFor.SpecialPrice)
            {
                columnsArray = typeof(SpecialPriceImport).GetProperties();

                string[] arra = { "ProductCode", "Price", "StartTime" };
                RequiredFields = arra.ToList();


            }

            List<FieldInfo> fieldInfoList = new List<FieldInfo>();
            foreach (var col in columnsArray)
            {
                if (col.Name.ToUpper() != "ID" && col.Name.ToUpper() != "COMPANYID" && col.Name.ToUpper() != "ITEM")
                {
                    var type = col.PropertyType;
                    var underlyingType = Nullable.GetUnderlyingType(type);
                    var returnType = underlyingType ?? type;
                    FieldInfo fieldInfo = new FieldInfo();
                    fieldInfo.Name = col.Name;
                    fieldInfo.FieldType = returnType.Name;
                    fieldInfo.IsRequired = RequiredFields.Contains(col.Name);
                    fieldInfoList.Add(fieldInfo);
                }

            }

            //enumData = from col in columnsArray
            //           where col.Name.ToUpper() != "ID" && col.Name.ToUpper() != "COMPANYID" && col.Name.ToUpper() != "ITEM"
            //           select new FieldInfo()
            //           {
            //               Name = col.Name,
            //               FieldType = col.PropertyType.Name,
            //               IsRequired = RequiredFields.Contains(col.Name)
            //           };

            return fieldInfoList.OrderByDescending(x => x.IsRequired);
        }

        public async Task<BaseResponseBO> ImportData(ImportBaseClass model, MemoryStream stream, string path)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            ImportLog importLogModel = new ImportLog();
            string filename = "";
            ImportType importType = ImportType.None;
            try
            {
                if (model.FileList.Count > 0)
                {
                    filename = model.FileList[0].Name;

                    if (!string.IsNullOrEmpty(filename) && filename.EndsWith(".csv"))
                    {
                        importType = ImportType.CSV;
                    }
                    else
                    {
                        importType = ImportType.Excel;
                    }
                }

                ImportMapping mapping = new ImportMapping();
                DataTable table = new DataTable();
                if (importType == ImportType.Excel)
                    table = mapping.GetDataTableFromExcel(stream, true);
                if (importType == ImportType.CSV && !string.IsNullOrEmpty(model.ImportedFileName))
                {
                    path = model.ImportedFileName;
                    table = mapping.GetDataTableFromCSV(path, stream, true);
                }

                ImportFor importFor;
                if (Enum.TryParse<ImportFor>(model.ImportFor, out importFor))
                {
                    importLogModel.ImportType = importFor.ToString();
                    if (importFor == ImportFor.Product)
                    {
                        response = await ImportExcelProduct(model, table, importLogModel);
                    }

                    if (importFor == ImportFor.Customer)
                    {
                        response = await ImportExcelCustomer(model, table, importLogModel);
                    }
                    if (importFor == ImportFor.SpecialPrice)
                    {
                        response = await ImportExcelSpecialPrice(model, table, importLogModel);
                    }

                }

                if (importType == ImportType.CSV && !string.IsNullOrEmpty(model.ImportedFileName))
                {
                    if (File.Exists(model.ImportedFileName))
                    {
                        File.Delete(model.ImportedFileName);
                    }

                }
            }
            catch (Exception ex)
            {
                importLogModel.ModifiedOn = DateTime.UtcNow;
                importLogModel.IsSuccess = false;
                importLogModel.Response = ex.Message;
                await _importlog.UpdateAsync(importLogModel);
                response.IsSuccess = false;
                response.Message = ex.Message;
            }

            return response;
            // context.Items.Where(a => a.ItemId > 500).BatchDelete();

        }

        private async Task<BaseResponseBO> ImportExcelProduct(ImportBaseClass model, DataTable dt, ImportLog importLogModel)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            List<ProductImport> list = new List<ProductImport>();
            try
            {
                if (!string.IsNullOrEmpty(model.MappingTitle) || model.MappingId > 0)
                {
                    importLogModel = await SaveImportMapping(model, importLogModel);

                }
                if (model.MappingId > 0)
                {
                    var result = _companyDataImportMappings.GetWithConditions(x => x.CompanyId == model.MappingId).FirstOrDefault();
                    if (result != null)
                    {
                        var mappinglist = JsonSerializer.Deserialize<Dictionary<string, string>>(result.MappingDetails);

                        model.MappingFields = mappinglist;
                    }

                }
                foreach (DataRow row in dt.Rows)
                {
                    ProductImport productImport = new ProductImport();
                    foreach (var item in model.MappingFields)
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            var fieldInfo = model.TableColumns.Where(x => x.Name == item.Value).FirstOrDefault();
                            var val = GetExcelColumnValue(row[item.Key], fieldInfo);
                            if (fieldInfo.IsRequired && string.IsNullOrEmpty(val))
                            {
                                response.IsSuccess = false;
                                response.Message = $"{item.Key} is required column.NULL value can not acceptable.";
                                return response;
                            }
                            if (val != null)
                                productImport[item.Value] = val;

                        }
                    }
                    productImport.CompanyId = model.CompanyId;
                    list.Add(productImport);
                }
                _importHelper.SqlTruncateTable("ProductImport", model.CompanyId);
                //var olditems = _product.GetWithConditions(x => x.CompanyId == model.CompanyId).ToList();

                //await _product.BulkDelete(ImportFor.Product, olditems);



                await _product.BulkInsert(list);

                var resultsp = await _context.GetProcedures().sp_SyncAPIProductsAsync(model.CompanyId);
                importLogModel.ModifiedOn = DateTime.UtcNow;
                importLogModel.IsSuccess = true;
                importLogModel.Response = "Import Success";
                await _importlog.UpdateAsync(importLogModel);

                response.Message = "Import success.";

            }
            catch (Exception ex)
            {
                importLogModel.ModifiedOn = DateTime.UtcNow;
                importLogModel.IsSuccess = false;
                importLogModel.Response = "Import Failed: " + ex.Message;
                await _importlog.UpdateAsync(importLogModel);

                response.Message = "Import Failed: " + ex.Message;
                response.IsSuccess = false;
                return response;
            }
            return response;
        }

        private async Task<BaseResponseBO> ImportExcelCustomer(ImportBaseClass model, DataTable dt, ImportLog importLogModel)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            try
            {
                List<CustomerImport> list = new List<CustomerImport>();
                if (model.MappingId > 0)
                {
                    var result = _companyDataImportMappings.GetWithConditions(x => x.CompanyId == model.MappingId).FirstOrDefault();
                    if (result != null)
                    {
                        model.MappingFields = JsonSerializer.Deserialize<Dictionary<string, string>>(result.MappingDetails);
                    }
                }
                foreach (DataRow row in dt.Rows)
                {
                    CustomerImport custImport = new CustomerImport();
                    foreach (var item in model.MappingFields)
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            var fieldInfo = model.TableColumns.Where(x => x.Name == item.Value).FirstOrDefault();
                            var val = GetExcelColumnValue(row[item.Key], fieldInfo);
                            if (fieldInfo.IsRequired && string.IsNullOrEmpty(val))
                            {
                                response.IsSuccess = false;
                                response.Message = $"{item.Key} is required column.NULL value can not acceptable.";
                                return response;
                            }
                            if (val != null)
                                custImport[item.Value] = val;

                        }
                        // productImport[item.Value] = row[item.Key];
                    }
                    custImport.CompanyId = model.CompanyId;
                    list.Add(custImport);
                }

                //var olditems = _customer.GetWithConditions(x => x.CompanyId == model.CompanyId).ToList();

                //await _customer.BulkDelete(ImportFor.Customer, olditems);
                _importHelper.SqlTruncateTable("CustomerImport", model.CompanyId);
                if (!string.IsNullOrEmpty(model.MappingTitle))
                {
                    importLogModel = await SaveImportMapping(model, importLogModel);

                }

                await _customer.BulkInsert(list);

                var resultsp = await _context.GetProcedures().sp_SyncAPICustomersAsync(model.CompanyId);

                importLogModel.ModifiedOn = DateTime.UtcNow;
                importLogModel.IsSuccess = true;
                importLogModel.Response = "Import Success";
                await _importlog.UpdateAsync(importLogModel);

                response.Message = "Import success.";

                return response;
            }
            catch (Exception ex)
            {
                importLogModel.ModifiedOn = DateTime.UtcNow;
                importLogModel.IsSuccess = false;
                importLogModel.Response = "Import Failed: " + ex.Message;
                await _importlog.UpdateAsync(importLogModel);

                response.Message = "Import Failed: " + ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }

        private async Task<BaseResponseBO> ImportExcelSpecialPrice(ImportBaseClass model, DataTable dt, ImportLog importLogModel)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            try
            {
                List<SpecialPriceImport> list = new List<SpecialPriceImport>();
                if (model.MappingId > 0)
                {
                    var result = _companyDataImportMappings.GetWithConditions(x => x.CompanyId == model.MappingId).FirstOrDefault();
                    if (result != null)
                    {
                        model.MappingFields = JsonSerializer.Deserialize<Dictionary<string, string>>(result.MappingDetails);
                    }
                }
                foreach (DataRow row in dt.Rows)
                {
                    SpecialPriceImport custImport = new SpecialPriceImport();
                    foreach (var item in model.MappingFields)
                    {
                        if (!string.IsNullOrEmpty(item.Value))
                        {
                            var fieldInfo = model.TableColumns.Where(x => x.Name == item.Value).FirstOrDefault();
                            var val = GetExcelColumnValue(row[item.Key], fieldInfo);

                            if (fieldInfo.IsRequired && string.IsNullOrEmpty(val))
                            {
                                response.IsSuccess = false;
                                response.Message = $"{item.Key} is required column.NULL value can not acceptable.";
                                return response;
                            }
                            if (val != null)
                                custImport[item.Value] = val;

                        }
                        // productImport[item.Value] = row[item.Key];
                    }
                    custImport.CompanyId = model.CompanyId;
                    list.Add(custImport);
                }

                //var olditems = _customer.GetWithConditions(x => x.CompanyId == model.CompanyId).ToList();

                //await _customer.BulkDelete(ImportFor.Customer, olditems);
                _importHelper.SqlTruncateTable("SpecialPriceImport", model.CompanyId);
                if (!string.IsNullOrEmpty(model.MappingTitle))
                {
                    importLogModel = await SaveImportMapping(model, importLogModel);

                }

                await _specialPriceImport.BulkInsert(list);




                importLogModel.ModifiedOn = DateTime.UtcNow;
                importLogModel.IsSuccess = true;
                importLogModel.Response = "Import Success";
                await _importlog.UpdateAsync(importLogModel);

                response.Message = "Import success.";

                return response;
            }
            catch (Exception ex)
            {
                importLogModel.ModifiedOn = DateTime.UtcNow;
                importLogModel.IsSuccess = false;
                importLogModel.Response = "Import Failed: " + ex.Message;
                await _importlog.UpdateAsync(importLogModel);

                response.Message = "Import Failed: " + ex.Message;
                response.IsSuccess = false;
                return response;
            }
        }
        private async Task<ImportLog> SaveImportMapping(ImportBaseClass model, ImportLog importLogModel)
        {
            CompanyDataImportMapping companyDataImportMapping = new CompanyDataImportMapping();


            companyDataImportMapping = _companyDataImportMappings.GetWithConditions(x => x.Id == model.MappingId).FirstOrDefault();
            if (companyDataImportMapping != null)
            {
                companyDataImportMapping.CompanyId = model.CompanyId;
                companyDataImportMapping.ImportType = model.ImportFor ?? "";
                companyDataImportMapping.MappingName = model.MappingTitle;
                companyDataImportMapping.MappingDetails = JsonSerializer.Serialize(model.MappingFields);
                await _companyDataImportMappings.UpdateAsync(companyDataImportMapping);
            }
            else
            {
                companyDataImportMapping = new CompanyDataImportMapping();
                companyDataImportMapping.CompanyId = model.CompanyId;
                companyDataImportMapping.ImportType = model.ImportFor ?? "";
                companyDataImportMapping.MappingName = model.MappingTitle;
                companyDataImportMapping.MappingDetails = JsonSerializer.Serialize(model.MappingFields);
                await _companyDataImportMappings.AddAsync(companyDataImportMapping);
            }




            importLogModel.MappingId = model.MappingId ?? companyDataImportMapping.Id;
            importLogModel.CreatedOn = DateTime.UtcNow;
            //importLogModel.Imp
            return await _importlog.AddAsync(importLogModel);
        }

        public ImportBaseClass GetMappingById(int MappingId)
        {
            ImportBaseClass importBaseClass = new ImportBaseClass();
            var result = _companyDataImportMappings.GetWithConditions(x => x.Id == MappingId).FirstOrDefault();
            if (result != null)
            {
                importBaseClass.MappingFields = JsonSerializer.Deserialize<Dictionary<string, string>>(result.MappingDetails);
            }
            importBaseClass.MappingTitle = result.MappingName;
            return importBaseClass;
        }
        public List<string> GetImportedSheetColumns(string path, ImportBaseClass model, MemoryStream stream)
        {
            ImportMapping mapping = new ImportMapping();
            string filename = "";
            ImportType importType = ImportType.None;
            if (model.FileList.Count > 0)
            {
                filename = model.FileList[0].Name;

                if (!string.IsNullOrEmpty(filename) && filename.EndsWith(".csv"))
                {
                    importType = ImportType.CSV;
                }
                else
                {
                    importType = ImportType.Excel;
                }
            }
            if (importType == ImportType.CSV)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                path = path + "//" + Guid.NewGuid() + Path.GetExtension(filename);
                model.ImportedFileName = path;
                using (var ms = new MemoryStream(model.bytearray))
                {
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        ms.WriteTo(fs);
                    }
                }
            }

            return mapping.GetImportedMapping(path, (int)importType, stream);
        }

        public dynamic? GetExcelColumnValue(dynamic val, FieldInfo fieldInfo)
        {
            if (val == null)
            {
                return null;
            }

            if (fieldInfo.FieldType == "Double")
            {
                return _importHelper.ValidateDoubleValue(val.ToString().Trim());
            }
            else if (fieldInfo.FieldType == "Int64")
            {
                return _importHelper.ValidateInt64Value(val.ToString().Trim());
            }
            else if (fieldInfo.FieldType == "Int32")
            {
                return _importHelper.ValidateInt32Value(val.ToString().Trim());
            }
            else if (fieldInfo.FieldType == "Boolean")
            {
                return _importHelper.ValidateBooleanValue(val.ToString().Trim());
            }
            else if (fieldInfo.FieldType == "DateTime")
            {
                return _importHelper.ValidateDateTimeValue(val.ToString().Trim());
            }
            else
            {
                return _importHelper.ValidatestringValue(val.ToString().Trim());
            }
        }
    }
}
