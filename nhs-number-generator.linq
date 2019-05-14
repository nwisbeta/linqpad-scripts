<Query Kind="Program" />

//Script to generate NHS numbers for testing
void Main()
{	
	var generator = new NhsNumberGenerator();
	
	//Random - useful if only a small amount are needed NHS number
	generator.GetRandomNhsNumber().Dump();

	//Sequential - useful when generating a large quantity  
	int numRequired = 20;
	generator.GetNhsNumbersSequentially(numRequired,37474736).Dump();	

}


public class NhsNumberGenerator {

	private StringBuilder nhsNumber = new StringBuilder();
	Random rnd = new Random();
	
	public string GetRandomNhsNumber(){
	
		nhsNumber.Clear();		
		int checksum = 0, digit;	
		
		//First 8 digits: Any random digits
		for (var i = 1; i <= 8; i++)
		{
			digit = rnd.Next(0, 9);
			checksum += (11 - i) * digit;
			AddDigit(digit);
		}	
		
		//9th digit: Any digit that doesn't make checksum = 1 (mod 11)
		do {
			digit = rnd.Next(0, 9);
		} 
		while ((checksum + 2 * digit) % 11 == 1);		
		checksum += 2 * digit;
		AddDigit(digit);
			
		//10th digit: checksum - 11	(mod 11)
		digit = (11 - checksum % 11) % 11;
		AddDigit(digit);
		
		return nhsNumber.ToString();		
		
	}	
		
	public List<string> GetNhsNumbersSequentially(long minQuantity, long firstEightDigits = 80000000)
	{
	
		List<string> allNumbers = new List<string>();
		long numberGenerated = 0;
		while (numberGenerated <= minQuantity)
		{			
			nhsNumber.Clear();		
			int digit, i = 1, checksum = 0;
			
			//First 8 digits, take from sequence position
			foreach (var n in firstEightDigits.ToString())
			{
				digit = (int)Char.GetNumericValue(n);
				checksum += (11 - i) * digit;
				AddDigit(digit);
				i++;
			}				
	
			//9th digit: First digit that doesn't make checksum = 1 (mod 11)
			numberGenerated += AddAllValidForFirstEight(allNumbers, checksum);	
			firstEightDigits++;
		}
		
		return allNumbers;	
		
	}	
	
	private void AddDigit(int digit){		
		if (nhsNumber.Length == 3 || nhsNumber.Length == 7)
			nhsNumber.Append(" ");		
		nhsNumber.Append(digit);		
	}
	
	private int AddAllValidForFirstEight(List<string> allNumbers, int firstEightChecksum)
	{
			int digit, checksum, added = 0;
			for (var n = 0; n < 10; n++) 
			{
				digit = n;
				checksum = firstEightChecksum;
				
				if ((checksum + 2 * digit) % 11 != 1)
				{
					checksum += 2 * digit;
					AddDigit(digit);
					
					digit = (11 - checksum % 11) % 11;
					AddDigit(digit);
					
					allNumbers.Add(nhsNumber.ToString());	
					nhsNumber.Length -= 2;
					added++;
				}
			}
			return added;
	}
	
}