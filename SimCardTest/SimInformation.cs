using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.NetworkOperators;

namespace SimCardTest
{
    public class SimInformation : ISimInformation
    {
        public IReadOnlyList<SimCard> GetSimCards()
        {
            var results = new List<SimCard>();

            var modem = MobileBroadbandModem.GetDefault();
            if (modem == null)
            {
                return results.AsReadOnly();
            }

            var account = modem.CurrentAccount;
            if (account == null)
            {
                return results.AsReadOnly();
            }
            var simCard = new SimCard();
            simCard.ICCID = account.CurrentDeviceInformation.SimIccId;
            simCard.IMSI = account.CurrentDeviceInformation.SubscriberId;
            simCard.MSISDN = modem.DeviceInformation.TelephoneNumbers;

            simCard.MCC = ExtractMCC(simCard.IMSI);
            simCard.MNC = ExtractMNC(simCard.IMSI);
            simCard.MSID = ExtractMSID(simCard.IMSI);

            results.Add(simCard);

            return results.AsReadOnly();
        }
        private static string ExtractMCC(string imsi)
        {
            if (string.IsNullOrWhiteSpace(imsi)) return string.Empty;

            var operatorId = imsi.Substring(0, 5);
            var mccId = operatorId.Substring(0, 3);

            return mccId;
            ;
        }

        private static string ExtractMSID(string imsi)
        {
            if (string.IsNullOrWhiteSpace(imsi)) return string.Empty;

            var msid = imsi.Substring(6);

            return msid;
        }
        private static string ExtractMNC(string imsi)
        {
            if (string.IsNullOrWhiteSpace(imsi)) return string.Empty;
            var operatorId = imsi.Substring(0, 5);
            var mncId = operatorId.Substring(3, 2);

            return mncId;
        }
    }
}
