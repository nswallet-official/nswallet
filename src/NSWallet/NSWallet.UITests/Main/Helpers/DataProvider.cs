using System;
using System.Text;
namespace NSWallet.UITests
{
	public class DataProvider
	{
		//Exclamation mark - ! - UTF-8 code
		public static readonly int LOWEST = 0x21;

		//Latin letter A - ! - UTF-8 code
		public static readonly int LOWEST_LETTER = 0x41;

		//Tilda - ~ - UTF-8 code
		public static readonly int HIGHEST = 0x7e;

		//Latin letter z - ! - UTF-8 code
		public static readonly int HIGHEST_LETTER = 0x7a;

		private Random random = new Random();

		public string GenerateRandomPassword()
		{
			int passwordLength = random.Next(3, 31);

			StringBuilder builder = new StringBuilder();
			int randomUpperBound = HIGHEST - LOWEST + 1;

			for (int i = 0; i < passwordLength; i++)
			{
				int rand = random.Next(randomUpperBound);
				builder.Append((char)(rand + LOWEST));
			}
			return builder.ToString();
		}

		public string GenerateRandomString()
		{
			int stringLength = random.Next(3, 10);

			StringBuilder builder = new StringBuilder();
			int randomUpperBound = HIGHEST_LETTER - LOWEST_LETTER;

			for (int i = 0; i < stringLength; i++)
			{
				int rand = random.Next(randomUpperBound);
				builder.Append((char)(rand + LOWEST_LETTER));
			}
			return builder.ToString();
		}

        public string GenerateRandomCardNumber()
        {
            string cardNumber = string.Empty;
            for (int i = 0; i < 16; i++)
                cardNumber = String.Concat(cardNumber, random.Next(10).ToString());
            return cardNumber;
        }

        public DateTime GenerateRandomDate()
		{
            int randYear = random.Next(0, 9); 
			DateTime randomDate = DateTime.Now.AddYears(+randYear);
            return randomDate;
		}

		public string GenerateRandomPhoneNumber()
		{
			string phoneNumber = "+375";
			for (int i = 0; i < 10; i++)
				phoneNumber = String.Concat(phoneNumber, random.Next(10).ToString());
			return phoneNumber;
		}

        public string GenerateRandomPinCodeNumber()
        {
            string pinCodeNumber = string.Empty;
            for (int i = 0; i < 4; i++)
                pinCodeNumber = String.Concat(pinCodeNumber, random.Next(10).ToString());
            return pinCodeNumber;
        }

        public DateTime GenerateRandomTime()
        {
            DateTime randomTime = DateTime.Now.AddHours(1);
            return randomTime;
        }
	}
}