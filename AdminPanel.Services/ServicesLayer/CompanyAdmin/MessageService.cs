
using AdminPanel.DataModel.Models;
using AdminPanel.Services.Repository;
using AdminPanel.Shared.BO;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public class MessageService : IMessageService
    {
        private readonly IGenericRepository<Message> _message;
        private readonly IGenericRepository<Customer> _customer;
        private readonly IGenericRepository<Group> _group;
        private readonly IMapper _mapper;

        public MessageService(IMapper mapper,
                              IGenericRepository<Message> message,
                              IGenericRepository<Customer> customer,
                              IGenericRepository<Group> group)
        {
            _mapper = mapper;
            _message = message;
            _customer = customer;
            _group = group;
        }

        public async Task<BaseResponseBO> SendMessage(MessageBO model)
        {
            var response = new BaseResponseBO() { IsSuccess = true };
            try
            {
                List<long> userIds = new List<long>();
                List<string> phoneNumbers = new List<string>();
                List<string> deviceTokens = new List<string>();

                var message = new Message();

                _mapper.Map(model, message);

                if (model.GroupId > 0)
                {
                    var group = await _group.GetSingleWithConditions(x => x.Id == model.GroupId, "GroupDetails");
                    model.CustomerIds = group.GroupDetails.Select(x => x.ReferenceId).ToList();
                }

                var customers = _customer.GetWithConditions(x => model.CustomerIds.Contains(x.CustomerId), "Users,Users.UserDeviceLocationAndTokens").ToList();

                foreach (var cust in customers)
                {
                    var users = cust.Users.Select(s => s.UserId);
                    userIds.AddRange(users);

                    var phones = cust.Users.Select(s => s.Phone);
                    phoneNumbers.AddRange(phones);

                    foreach (var user in cust.Users)
                    {
                        var tokens = user.UserDeviceLocationAndTokens.Where(x => x.DeviceType == "Android" || x.DeviceType == "iPhone").Select(s => s.DeviceToken);
                        deviceTokens.AddRange(tokens);
                    }
                }

                // add the records to the message recipient table
                foreach (var item in userIds)
                {
                    var recipient = new MessageRecipient() { UserId = item, IsRead = false };
                    message.MessageRecipients.Add(recipient);
                }
                await _message.AddAsync(message);

                if (model.IsSms)
                {
                    response.Message += SendSMS(phoneNumbers) + Environment.NewLine;
                }
                response.Message += SendPushNotification(deviceTokens) + Environment.NewLine;

                return response;
            }
            catch (Exception ex)
            {
                return new BaseResponseBO()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        private string SendSMS(List<string> phoneNumbers)
        {
            return $"SMS sent to {phoneNumbers.Count} users";
        }

        private string SendPushNotification(List<string> tokens)
        {
            return $"Push Notification sent to {tokens.Count} users";
        }
    }
}
