using System;
using System.Security.Cryptography;

namespace NSWallet.NetStandard.Helpers.FA
{
	public class Totp
	{
		const long unixEpochTicks = 621355968000000000L;

		const long ticksToSeconds = 10000000L;

		private const double step = 30.0;

		private const int totpSize = 6;

		private byte[] key;

		public Totp(byte[] secretKey)
		{
			key = secretKey;
		}

		public string ComputeTotp()
		{
			var window = CalculateTimeStepFromTimestamp(DateTime.UtcNow);

			var data = GetBigEndianBytes(window);

			var hmac = new HMACSHA1();
			hmac.Key = key;
			var hmacComputedHash = hmac.ComputeHash(data);

			int offset = hmacComputedHash[hmacComputedHash.Length - 1] & 0x0F;
			var otp = (hmacComputedHash[offset] & 0x7f) << 24
				   | (hmacComputedHash[offset + 1] & 0xff) << 16
				   | (hmacComputedHash[offset + 2] & 0xff) << 8
				   | (hmacComputedHash[offset + 3] & 0xff); // % 1000000;

			var result = Digits(otp, totpSize);

			return result;
		}

		public double RemainingSeconds()
		{
			return step - ((((double)DateTime.UtcNow.Ticks - (double)unixEpochTicks) / (double)ticksToSeconds) % step);
		}

		private byte[] GetBigEndianBytes(long input)
		{
			// Since .net uses little endian numbers, we need to reverse the byte order to get big endian.
			var data = BitConverter.GetBytes(input);
			Array.Reverse(data);
			return data;
		}

		private long CalculateTimeStepFromTimestamp(DateTime timestamp)
		{
			var unixTimestamp = (timestamp.Ticks - unixEpochTicks) / ticksToSeconds;
			var window = unixTimestamp / (long)step;
			return window;
		}

		private string Digits(long input, int digitCount)
		{
			var truncatedValue = ((int)input % (int)Math.Pow(10, digitCount));
			return truncatedValue.ToString().PadLeft(digitCount, '0');
		}
	}
}