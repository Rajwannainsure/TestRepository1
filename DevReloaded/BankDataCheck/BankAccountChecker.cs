using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace BankDataCheck
{
    public static class BankAccountChecker
    {
        public enum CheckReturnCode
        {
            SuccessfulValidation = 1,
            SortCodeNotOnUKDirectory = 2,
            ValidationFailedButSortCodeExists = 3,
            InvalidSortCode = 4,
            InvalidAccountNumber = 5,
            InternalSystemError = 6,
            ProductNotRegistered = 7
        }

        public static CheckReturnCode Check(string accountNumber, string sortCode)
        {
            ch_com.ch_com accountDataChecker = new ch_com.ch_com();
            string validateResultString = accountDataChecker.Validate(
                accountNumber, sortCode, "outASPXML.txt");
            XElement xmlElementValidateResult = (validateResultString == null) ? 
                null : XElement.Parse(validateResultString);
            XElement xmlElementReturnCode = (xmlElementValidateResult == null) ?
                null : xmlElementValidateResult.Element("ReturnCode");
            string returnCodeString = (xmlElementReturnCode == null) ?
                string.Empty : xmlElementReturnCode.Value;

            CheckReturnCode returnCodeEnum;
            switch (returnCodeString)
            {
                case "1":
                case "2":
                case "3":
                case "4":
                case "5":
                    returnCodeEnum = (CheckReturnCode)int.Parse(returnCodeString);
                    break;

                case "X":
                    returnCodeEnum = CheckReturnCode.ProductNotRegistered;
                    break;

                default:
                    returnCodeEnum = CheckReturnCode.InternalSystemError;
                    break;
            }

            return returnCodeEnum;
        }
    }
}
