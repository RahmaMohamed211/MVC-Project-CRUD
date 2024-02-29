using Company.DAL.models;
using Twilio.Rest.Api.V2010.Account;

namespace company.PL.Helper
{
    public interface ISmsMessage
    {
        MessageResource Send(SMS sms); 
    }
}
