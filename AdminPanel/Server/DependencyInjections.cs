using AdminPanel.Services.Repository.Account;
using AdminPanel.Services.Repository.MasterAdmin;
using AdminPanel.Services.Repository;
using AdminPanel.Services.ServicesLayer.CompanyAdmin;
using AdminPanel.Services.ServicesLayer;
using AdminPanel.Services;

namespace AdminPanel.Server
{
	public static class DependencyInjections
	{
		public static void AddServiceRepos(this IServiceCollection services)
		{
			// Add repository DI's
			services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<IAccountRepository, AccountRepository>();
			services.AddScoped<IMasterTablesRepository, MasterTablesRepository>();

			// Add Service DI's
			services.AddScoped<ICommonService, CommonService>();
			services.AddScoped<ICompanyService, CompanyService>();
			services.AddScoped<IPackageService, PackageService>();
			services.AddScoped<ICategoriesService, CategoriesService>();
			services.AddScoped<ICustomerService, CustomerService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IGroupService, GroupService>();
			services.AddScoped<IMarketingService, MarketingService>();
			services.AddScoped<IMessageService, MessageService>();
			services.AddScoped<IDiscountService, DiscountService>();
			services.AddScoped<IimportDataService, ImportDataService>();
			services.AddScoped<ICutoffService, CutoffService>();
			services.AddScoped<ISettings, Settings>();
			services.AddScoped<IOrderAdminService, OrderAdminService>();
			services.AddScoped<IOrderRepository, OrderRepository>();
			services.AddScoped<IAttributeService, AttributeService>();
		}
	}
}
