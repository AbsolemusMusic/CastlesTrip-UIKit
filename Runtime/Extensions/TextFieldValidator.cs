using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CT.Extensions
{
    public class TextFieldValidator
    {

        public void CheckValid(string value, TextFieldType type, TextFieldValidateCompletion completion)
        {
            switch (type)
            {
                case TextFieldType.email:
                    break;

                case TextFieldType.nickname:
                    break;

                case TextFieldType.password:
                    CheckPass(value, completion);
                    break;
            }
        }

        private void CheckEmail(string value, TextFieldValidateCompletion completion)
        {

        }

        private void CheckNickname(string value, TextFieldValidateCompletion completion)
        {

        }

        private void CheckPass(string value, TextFieldValidateCompletion completion)
        {
            bool isLower = false;
            bool isUpper = false;
            bool isNumber = false;
            bool isMore6 = false;

            isMore6 = isMore6 || value.Length >= 6;
            foreach (char ch in value)
            {
                isLower = isLower || (char.IsLetter(ch) && char.IsLower(ch));
                isUpper = isUpper || (char.IsLetter(ch) && char.IsUpper(ch));
                isNumber = isNumber || char.IsNumber(ch);
            }

            string description = "Password must be";

            if (!isLower) description += " lowercase,";
            if (!isUpper) description += " uppercase,";
            if (!isNumber) description += " number,";
            if (!isMore6) description += " more 6 chars,";
            if (!isLower || !isUpper || !isNumber || !isMore6)
            {
                description += " please fix!";
                completion?.Invoke(description, false);
                return;
            }

            description = "Fine, ok";
            completion?.Invoke(description, true);
        }
    }

    public delegate void TextFieldValidateCompletion(string description, bool isOk);

    public enum TextFieldType
    {
        email, nickname, password
    }

    public interface ITextFieldValidateModel
    {

    }
}