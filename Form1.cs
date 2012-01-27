//  Program: Random Poker Hand Creator
//  Description: Creates an arff file for a given number of poker hands.

using System;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Security.Cryptography;

namespace PokerTrainingArffFileCreator
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void btnGenerate_Click(object sender, EventArgs e)
		{
			if (txtItemCount.Text != "")
			{
				int iNum = Convert.ToInt32(txtItemCount.Text);
				if (iNum > 0)
				{
					StringBuilder sbArffFile = new StringBuilder();

					sbArffFile.Append("@RELATION poker-hand");
					sbArffFile.AppendLine();
					sbArffFile.Append("@ATTRIBUTE R1 {1,2,3,4,5,6,7,8,9,10,11,12,13}");
					sbArffFile.AppendLine();
					sbArffFile.Append("@ATTRIBUTE R2 {1,2,3,4,5,6,7,8,9,10,11,12,13}");
					sbArffFile.AppendLine();
					sbArffFile.Append("@ATTRIBUTE R3 {1,2,3,4,5,6,7,8,9,10,11,12,13}");
					sbArffFile.AppendLine();
					sbArffFile.Append("@ATTRIBUTE R4 {1,2,3,4,5,6,7,8,9,10,11,12,13}");
					sbArffFile.AppendLine();
					sbArffFile.Append("@ATTRIBUTE R5 {1,2,3,4,5,6,7,8,9,10,11,12,13}");
					sbArffFile.AppendLine();
					sbArffFile.Append("@ATTRIBUTE SameSuit {TRUE,FALSE}");
					sbArffFile.AppendLine();
					sbArffFile.Append("@ATTRIBUTE hand {0,1,2,3,4,5,6,7,8,9}");
					sbArffFile.AppendLine();
					sbArffFile.Append("@DATA");
					sbArffFile.AppendLine();

					for (int x = 0; x < iNum; x++)
					{
						sbArffFile.Append(GetAnotherHand());
						sbArffFile.AppendLine();
					}

					if (File.Exists(iNum.ToString() + "TrainingHands.arff"))
						File.Delete(iNum.ToString() + "TrainingHands.arff");

					FileStream fs = new FileStream(iNum.ToString() + "TrainingHands.arff", FileMode.CreateNew, FileAccess.Write);

					Byte[] bytes = System.Text.Encoding.ASCII.GetBytes(sbArffFile.ToString());

					foreach (Byte b in bytes)
						fs.WriteByte(b);

					fs.Close();
				}
			}
		}

		private string GetAnotherHand()
		{
			StringBuilder sbHand = new StringBuilder();
			RandomNumberGenerator rnGen = new RNGCryptoServiceProvider();
			byte[] randomNumber = new byte[1];

			int iHandType, iTemp;
			int[] iHandCards = new int[5];
			string sSameSuit = "";
			bool bNeedsNewCard, bGoodHand;

			rnGen.GetBytes(randomNumber);
			iHandType = Convert.ToInt32(randomNumber[0]) % 10;
			iHandCards[0] = 0;
			iHandCards[1] = 0;
			iHandCards[2] = 0;
			iHandCards[3] = 0;
			iHandCards[4] = 0;
			iTemp = 0;
			bNeedsNewCard = true;
			bGoodHand = false;

			switch (iHandType)
			{
				case 0:     // High Card
					while (!bGoodHand)
					{
						rnGen.GetBytes(randomNumber);
						iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
						for (int a = 1; a < 5; a++)
						{
							while (bNeedsNewCard)
							{
								bNeedsNewCard = false;
								rnGen.GetBytes(randomNumber);
								iTemp = Convert.ToInt32(randomNumber[0]) % 13 + 1;
								for (int b = (a - 1); b >= 0; b--)
								{
									if (iHandCards[b] == iTemp)
									{
										bNeedsNewCard = true;
										break;
									}
								}
							}
							iHandCards[a] = iTemp;
							bNeedsNewCard = true;
						}
						bGoodHand = true;
						if ((iHandCards.Max() - iHandCards.Min()) == 4)
						{
							bGoodHand = false;
						}
						else if (iHandCards.Min() == 1)
						{
							bGoodHand = false;
							foreach (int i in iHandCards)
							{
								if (i != 1 && i != 10 && i != 11 && i != 12 && i != 13)
								{
									bGoodHand = true;
									break;
								}
							}
						}
					}
					sSameSuit = "FALSE";
					break;
				case 1:     // Pair
					rnGen.GetBytes(randomNumber);
					iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
					iHandCards[1] = iHandCards[0];
					for (int a = 2; a < 5; a++)
					{
						while (bNeedsNewCard)
						{
							bNeedsNewCard = false;
							rnGen.GetBytes(randomNumber);
							iTemp = Convert.ToInt32(randomNumber[0]) % 13 + 1;
							for (int b = (a - 1); b >= 0; b--)
							{
								if (iHandCards[b] == iTemp)
								{
									bNeedsNewCard = true;
									break;
								}
							}
						}
						iHandCards[a] = iTemp;
						bNeedsNewCard = true;
					}
					sSameSuit = "FALSE";
					break;
				case 2:     // Two Pair
					rnGen.GetBytes(randomNumber);
					iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
					iHandCards[1] = iHandCards[0];
					do
					{
						rnGen.GetBytes(randomNumber);
						iHandCards[2] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
					}
					while (iHandCards[2] == iHandCards[0]);
					iHandCards[3] = iHandCards[2];

					while (bNeedsNewCard)
					{
						bNeedsNewCard = false;
						rnGen.GetBytes(randomNumber);
						iTemp = Convert.ToInt32(randomNumber[0]) % 13 + 1;
						for (int b = 3; b >= 0; b--)
						{
							if (iHandCards[b] == iTemp)
							{
								bNeedsNewCard = true;
								break;
							}
						}
					}
					iHandCards[4] = iTemp;

					sSameSuit = "FALSE";
					break;
				case 3:     // Three of a Kind
					rnGen.GetBytes(randomNumber);
					iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
					iHandCards[1] = iHandCards[0];
					iHandCards[2] = iHandCards[0];
					for (int a = 3; a < 5; a++)
					{
						while (bNeedsNewCard)
						{
							bNeedsNewCard = false;
							rnGen.GetBytes(randomNumber);
							iTemp = Convert.ToInt32(randomNumber[0]) % 13 + 1;
							for (int b = (a - 1); b >= 0; b--)
							{
								if (iHandCards[b] == iTemp)
								{
									bNeedsNewCard = true;
									break;
								}
							}
						}
						iHandCards[a] = iTemp;
						bNeedsNewCard = true;
					}
					sSameSuit = "FALSE";
					break;
				case 4:     // Straight
					rnGen.GetBytes(randomNumber);
					iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 10 + 1;
					iHandCards[1] = iHandCards[0] + 1;
					iHandCards[2] = iHandCards[1] + 1;
					iHandCards[3] = iHandCards[2] + 1;
					if (iHandCards[0] == 10)
					{
						iHandCards[4] = 1;
					}
					else
					{
						iHandCards[4] = iHandCards[3] + 1;
					}
					sSameSuit = "FALSE";
					break;
				case 5:     // Flush
					while (!bGoodHand)
					{
						rnGen.GetBytes(randomNumber);
						iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
						for (int a = 1; a < 5; a++)
						{
							while (bNeedsNewCard)
							{
								bNeedsNewCard = false;
								rnGen.GetBytes(randomNumber);
								iTemp = Convert.ToInt32(randomNumber[0]) % 13 + 1;
								for (int b = (a - 1); b >= 0; b--)
								{
									if (iHandCards[b] == iTemp)
									{
										bNeedsNewCard = true;
										break;
									}
								}
							}
							iHandCards[a] = iTemp;
							bNeedsNewCard = true;
						}
						bGoodHand = true;
						if ((iHandCards.Max() - iHandCards.Min()) == 4)
						{
							bGoodHand = false;
						}
						else if (iHandCards.Min() == 1)
						{
							bGoodHand = false;
							foreach (int i in iHandCards)
							{
								if (i != 1 && i != 10 && i != 11 && i != 12 && i != 13)
								{
									bGoodHand = true;
									break;
								}
							}
						}
					}
					sSameSuit = "TRUE";
					break;
				case 6:     // Full House
					rnGen.GetBytes(randomNumber);
					iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
					iHandCards[1] = iHandCards[0];
					iHandCards[2] = iHandCards[0];
					do
					{
						rnGen.GetBytes(randomNumber);
						iHandCards[3] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
					}
					while (iHandCards[3] == iHandCards[0]);
					iHandCards[4] = iHandCards[3];
					sSameSuit = "FALSE";
					break;
				case 7:     // Four of a Kind
					rnGen.GetBytes(randomNumber);
					iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
					iHandCards[1] = iHandCards[0];
					iHandCards[2] = iHandCards[0];
					iHandCards[3] = iHandCards[0];
					while (bNeedsNewCard)
					{
						bNeedsNewCard = false;
						rnGen.GetBytes(randomNumber);
						iTemp = Convert.ToInt32(randomNumber[0]) % 13 + 1;
						for (int b = 3; b >= 0; b--)
						{
							if (iHandCards[b] == iTemp)
							{
								bNeedsNewCard = true;
								break;
							}
						}
					}
					iHandCards[4] = iTemp;
					sSameSuit = "FALSE";
					break;
				case 8:     // Straight Flush
					rnGen.GetBytes(randomNumber);
					iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 9 + 1;
					iHandCards[1] = iHandCards[0] + 1;
					iHandCards[2] = iHandCards[1] + 1;
					iHandCards[3] = iHandCards[2] + 1;
					iHandCards[4] = iHandCards[3] + 1;
					sSameSuit = "TRUE";
					break;
				case 9:     // Royal Flush
					iHandCards[0] = 10;
					iHandCards[1] = 11;
					iHandCards[2] = 12;
					iHandCards[3] = 13;
					iHandCards[4] = 1;
					sSameSuit = "TRUE";
					break;
				default:    // Use High Card Hand
					while (!bGoodHand)
					{
						rnGen.GetBytes(randomNumber);
						iHandCards[0] = Convert.ToInt32(randomNumber[0]) % 13 + 1;
						for (int a = 1; a < 5; a++)
						{
							while (bNeedsNewCard)
							{
								bNeedsNewCard = false;
								rnGen.GetBytes(randomNumber);
								iTemp = Convert.ToInt32(randomNumber[0]) % 13 + 1;
								for (int b = (a - 1); b >= 0; b--)
								{
									if (iHandCards[b] == iTemp)
									{
										bNeedsNewCard = true;
										break;
									}
								}
							}
							iHandCards[a] = iTemp;
							bNeedsNewCard = true;
						}
						bGoodHand = true;
						if ((iHandCards.Max() - iHandCards.Min()) == 4)
						{
							bGoodHand = false;
						}
						else if (iHandCards.Min() == 1)
						{
							bGoodHand = false;
							foreach (int i in iHandCards)
							{
								if (i != 1 && i != 10 && i != 11 && i != 12 && i != 13)
								{
									bGoodHand = true;
									break;
								}
							}
						}
					}
					sSameSuit = "FALSE";
					break;
			};

			#region RandomOrder

			//int iCurNum = 0;
			//int[] iFinalHand = new int[5];

			//iFinalHand[0] = 0;
			//iFinalHand[1] = 0;
			//iFinalHand[2] = 0;
			//iFinalHand[3] = 0;
			//iFinalHand[4] = 0;

			//while (iCurNum < 5)
			//{
			//    rnGen.GetBytes(randomNumber);
			//    iTemp = Convert.ToInt32(randomNumber[0]) % 5;
			//    //iTemp = randNum.Next(0, 4);
			//    if (iFinalHand[iTemp] == 0)
			//    {
			//        iFinalHand[iTemp] = iHandCards[iCurNum];
			//        iCurNum++;
			//    }
			//}

			#endregion

			#region SortedOrder

			for (int a = 0; a < 5; a++)
			{
				for (int b = (a + 1); b < 5; b++)
				{
					if (iHandCards[a] > iHandCards[b])
					{
						iTemp = iHandCards[b];
						iHandCards[b] = iHandCards[a];
						iHandCards[a] = iTemp;
					}
				}
			}

			#endregion


			for (int y = 0; y < 5; y++)
				sbHand.Append(iHandCards[y].ToString() + ",");
			//sbHand.Append(iFinalHand[y].ToString() + ",");

			sbHand.Append(sSameSuit + "," + iHandType.ToString());

			return sbHand.ToString();
		}
	}
}
