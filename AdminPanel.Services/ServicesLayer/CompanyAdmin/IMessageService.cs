using AdminPanel.Shared.BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdminPanel.Services.ServicesLayer
{
    public interface IMessageService
    {
        Task<BaseResponseBO> SendMessage(MessageBO model);
    }
}
