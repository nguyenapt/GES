using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GES.Common.Helpers
{
    public class IsinHelper
    {

        public static string GenerateIsinCode(string lastIsinCode)
        {
            if (string.IsNullOrWhiteSpace(lastIsinCode) || lastIsinCode.Length < 12)
            {
                return "C_" + "1".PadLeft(10, '0');
            }
            var numberPad = lastIsinCode.Substring(2);
                
            long lastNumber;
            if (long.TryParse(numberPad, out lastNumber))
            {
                return "C_" + (lastNumber + 1).ToString().PadLeft(10, '0');
            }

            return "C_" + "1".PadLeft(10, '0');
        }

        public static string GetLargerCustomIsin(string isin01, string isin02)
        {
            var num01 = int.Parse(isin01.Substring(2));
            var num02 = int.Parse(isin02.Substring(2));

            return num01 > num02 ? isin01 : isin02;
        }

        public bool CheckValidatorIsinCode(string isin)
        {
            // basic pattern
            var regex = @"(XS|AD|AE|AF|AG|AI|AL|AM|AO|AQ|AR|AS|AT|AU|AW|AX|AZ|BA|BB|BD|BE|BF|BG|BH|BI|BJ|BL|BM|BN|BO|BQ|BR|BS|BT|BV|BW|BY|BZ|CA|CC|CD|CF|CG|CH|CI|CK|CL|CM|CN|CO|CR|CU|CV|CW|CX|CY|CZ|DE|DJ|DK|DM|DO|DZ|EC|EE|EG|EH|ER|ES|ET|FI|FJ|FK|FM|FO|FR|GA|GB|GD|GE|GF|GG|GH|GI|GL|GM|GN|GP|GQ|GR|GS|GT|GU|GW|GY|HK|HM|HN|HR|HT|HU|ID|IE|IL|IM|IN|IO|IQ|IR|IS|IT|JE|JM|JO|JP|KE|KG|KH|KI|KM|KN|KP|KR|KW|KY|KZ|LA|LB|LC|LI|LK|LR|LS|LT|LU|LV|LY|MA|MC|MD|ME|MF|MG|MH|MK|ML|MM|MN|MO|MP|MQ|MR|MS|MT|MU|MV|MW|MX|MY|MZ|NA|NC|NE|NF|NG|NI|NL|NO|NP|NR|NU|NZ|OM|PA|PE|PF|PG|PH|PK|PL|PM|PN|PR|PS|PT|PW|PY|QA|RE|RO|RS|RU|RW|SA|SB|SC|SD|SE|SG|SH|SI|SJ|SK|SL|SM|SN|SO|SR|SS|ST|SV|SX|SY|SZ|TC|TD|TF|TG|TH|TJ|TK|TL|TM|TN|TO|TR|TT|TV|TW|TZ|UA|UG|UM|US|UY|UZ|VA|VC|VE|VG|VI|VN|VU|WF|WS|YE|YT|ZA|ZM|ZW|C_)([0-9A-Z]{9})([0-9])";
            var match = Regex.Match(isin, regex);
            
            if (match.Success)
            {
                if (isin.Substring(0,2) == "C_") // custom ISIN
                {
                    return true;
                }

                return isin.Substring(isin.Length - 1) == GenerateCheckDigit(isin.Substring(0, 11));
            }
            return false;
        }

        /// <summary>
        /// GenerateCheckDigit("US984121103")
        /// </summary>
        /// <param name="cusip"></param>
        /// <returns></returns>
        private string GenerateCheckDigit(string cusip)
        {
            int sum = 0;
            char[] digits = cusip.ToUpper().ToCharArray();
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ[\\]@#*^_";

            string newDigits = "";

            for (int i = 0; i < digits.Length; i++)
            {
                int val;


                if (!int.TryParse(digits[i].ToString(), out val))
                    val = alphabet.IndexOf(digits[i]) + 10;

                newDigits = newDigits + val.ToString();

            }

            char[] ndigits = newDigits.ToCharArray();

            Console.WriteLine(newDigits);

            for (int i = 0; i < ndigits.Length; i++)
            {
                int val;
                int.TryParse(ndigits[i].ToString(), out val);


                if ((i % 2) == 0)
                    val *= 2;

                if (val >= 10)
                {

                    val = (val % 10) + (val / 10);
                }


                sum += val;
            }

            int check = (10 - (sum % 10)) % 10;

            return check.ToString();
        }

    }
}
