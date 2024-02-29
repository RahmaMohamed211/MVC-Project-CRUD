using company.PL.Settings;
using Company.DAL.models;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace company.PL.Helper
{
    public class SmsSetting : ISmsMessage
    {
        private readonly TwilioSetting _options;

        public SmsSetting(IOptions<TwilioSetting> options)
        {
            _options = options.Value;
        }
        public MessageResource Send(SMS sms)
        {
            TwilioClient.Init(_options.AccountSID, _options.AuthToken);

            var result = MessageResource.Create
                (
                body: sms.Body,
                from: new Twilio.Types.PhoneNumber(_options.TwilioPhoneNumber),
                to: sms.PhoneNumber

                );
            return result;



        }
    }
}
