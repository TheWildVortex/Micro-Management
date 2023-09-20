using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Global;

public class Global : MonoBehaviour
{
    // GLOBAL STATES:
    private static bool isGamePaused = false;

    // GLOBAL DATA:
    // Names
    private static string[] firstNames = { "John", "Jane", "Michael", "Emily", "David", "Sarah" };
    private static string[] lastNames = { "Smith", "Johnson", "Williams", "Brown", "Davis", "Miller" };

    // Loan Details
    private static string[] businesses = { "TechCorp", "FoodTruck", "Consulting", "ArtStudio" };
    private static string[] busAddresses = { "123 Main Street", "456 Park Avenue", "789 Elm Road" };
    private static string[] homeAddresses = { "111 Elm Street", "222 Maple Avenue", "333 Oak Drive" };

    // Identification Details
    // 0 - "Driver's License", 1 - "Senior Citizen ID", 2 - "National ID", 3 - "Barangay ID", 4 - "Passport", 5 - "Elementary ID", 6 - "High School ID"
    private static int[] idTypesValid = { 0, 1, 2, 3, 4 }; 
    private static int[] idTypesInvalid = { 0, 1, 2, 3, 4, 5, 6 };
    private static string[] idNationalities = { "American", "Korean", "Japanese" };
    private static string[] idSexes = { "Male", "Female" };
    private static string[] idCivilStatus = { "Single", "Married", "Separated", "Civil Partnership", "Widowed" };
    private static string[] idBirthPlace = { "Manila", "Bacolod", "Cebu", "Davao", "Baguio" };

    // Driver's License Details
    private static string[] idBloodTypes = { "A+", "A-", "B+", "B-", "O+", "O-", "AB+", "AB-" };

    // Barangay ID Details
    private static string[] idBarangays = { "Taculing", "Villamonte", "Sum-ag", "Mansilingan", "Vista Alegre", "Alijis", "Granada", "Estefania", "Handumanan", "Singcang-Airport" };

    // School ID Details
    private static string[] idElemSchools = { "Taculing", "Villamonte", "Sum-ag", "Mansilingan", "Vista Alegre", "Alijis", "Granada", "Estefania", "Handumanan", "Singcang-Airport" };
    private static string[] idHighSchools = { "Bacolod City National", "UP Integrated", "Philippine Science", "Sta. Lucia", "Calumpang", "Estefania National", "Negros Occidental", "Poveda Learning Center", "Colegio San Agustin" };
    private static string[] idSchoolCities = { "Bacolod", "Bago", "Binalbagan", "Cadiz", "Calatrava", "Candoni", "Cauayan", "Enrique B. Magalona", "Escalante", "Himamaylan", "Hinigaran", "Hinoba-an", "Ilog", "Isabela", "Kabankalan", "La Carlota", "La Castellana", "Manapla", "Moises Padilla", "Murcia", "Pontevedra", "Pulupandan", "Sagay", "Salvador Benedicto", "San Carlos", "San Enrique", "Silay", "Sipalay", "Talisay", "Toboso", "Valladolid" };

    // Bank Slip Details
    private static string[] idBankBranches = { "Bacolod", "Bago", "Binalbagan", "Cadiz", "Calatrava", "Candoni", "Cauayan", "Enrique B. Magalona", "Escalante", "Himamaylan", "Hinigaran", "Hinoba-an", "Ilog", "Isabela", "Kabankalan", "La Carlota", "La Castellana", "Manapla", "Moises Padilla", "Murcia", "Pontevedra", "Pulupandan", "Sagay", "Salvador Benedicto", "San Carlos", "San Enrique", "Silay", "Sipalay", "Talisay", "Toboso", "Valladolid" };

    // Game Data
    public static Player PlayerData { get; set; }
    public static List<Customer> CustomersData { get; set; }

    // Singleton instances
    public static Global Instance;

    // Ensure Singelton instance
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // DATA GENERATORS/CALCULATORS:
    // Random Date Generator
    public static class RandomDateGenerator
    {
        private static readonly System.Random random = new System.Random();

        public static System.DateTime GenerateRandomDate(System.DateTime startDate, System.DateTime endDate)
        {
            System.TimeSpan timeSpan = endDate - startDate;
            System.TimeSpan randomTimeSpan = new System.TimeSpan((long)(random.NextDouble() * timeSpan.Ticks));
            return startDate + randomTimeSpan;
        }
    }

    // Random Number Generator
    public static class RandomNumberGenerator
    {
        // Common data
        private static readonly System.Random random = new System.Random();
        private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        private static string[] sectionColors = { "Red", "Blue", "Green", "Yellow", "Orange", "Purple", "Pink", "Black", "White" };

        // Generate Random Number
        public static int GenerateRandomNumber(int numberOfDigits)
        {
            if (numberOfDigits <= 0)
            {
                throw new ArgumentException("Number of digits must be greater than 0");
            }

            int maxNumber = 1;
            for (int i = 0; i < numberOfDigits; i++)
            {
                maxNumber *= 10;
            }
            maxNumber -= 1;

            int randomNumber = random.Next(0, maxNumber + 1);

            return randomNumber;
        }

        // Generate Random Phone Number
        public static string GenerateRandomPhoneNumber(int numberOfDigits)
        {
            if (numberOfDigits <= 0)
            {
                throw new ArgumentException("Number of digits must be greater than 0");
            }

            int maxNumber = (int)Math.Pow(10, numberOfDigits) - 1;
            int randomNumber = random.Next(0, maxNumber);

            string phoneNumber = randomNumber.ToString().PadLeft(numberOfDigits, '0');
            phoneNumber = phoneNumber.Insert(3, "-").Insert(7, "-");

            return phoneNumber;
        }

        // Generate Random Driver's License Number
        public static string GenerateRandomLicenseNumber()
        {
            int numberPart = random.Next(3, 1000); // Random number between 3 and 999
            int secondPart = random.Next(10, 100); // Random number between 10 and 99
            int thirdPart = random.Next(100000, 1000000); // Random number between 100000 and 999999

            string alphanumericPart = Guid.NewGuid().ToString("N").Substring(0, 3); // Generate a random alphanumeric part

            return $"{numberPart:000}-{secondPart:00}-{thirdPart:000000}-{alphanumericPart}";
        }

        // Generate Random National ID Number
        public static string GenerateRandomNationalIDNumber()
        {
            string id = "";

            for (int i = 0; i < 4; i++)
            {
                id += GenerateRandomNumber(4).ToString("D4");

                if (i < 3)
                {
                    id += "-";
                }
            }

            return id;
        }

        // Generate Random Barangay ID Number
        public static string GenerateRandomBarangayIDNumber()
        {
            string id = "BSN-";

            id += GenerateRandomNumber(7).ToString("D7");
            id += "-";
            id += GenerateRandomNumber(4).ToString("D4");
            id += "-";
            id += GenerateRandomNumber(2).ToString("D2");

            return id;
        }

        // Generate Random Precinct Number
        public static string GenerateRandomPrecinctNumber()
        {
            int firstPart = random.Next(1, 9); // Random number between 1 and 9
            int secondPart = random.Next(1, 10000); // Random number between 1 and 9999

            return $"{firstPart}-{secondPart:0000}";
        }

        // Generate Random Passport Number
        public static string GenerateRandomPassportNumber()
        {
            int numberPart = random.Next(10000000, 99999999); // Random number between 10000000 and 99999999
            char letterPart = (char)random.Next('A', 'Z' + 1); // Random uppercase letter

            return $"P{numberPart:00000000}{letterPart}";
        }

        // Generate Random Section Name
        public static string GenerateSection(int grade)
        {
            if (grade < 1)
            {
                throw new ArgumentException("Invalid grade number");
            }

            string color = sectionColors[random.Next(sectionColors.Length)];

            return $"Grade {grade} - {color}";
        }

        // Generate School Year
        public static string GenerateSchoolYear(int currentYear, int gradeNumber, int currentAge)
        {
            int startYear, endYear;

            if (gradeNumber >= 1 && gradeNumber <= 3)
            {
                startYear = currentYear - (currentAge - 5);
                endYear = startYear + 1;
            }
            else if (gradeNumber >= 4 && gradeNumber <= 6)
            {
                startYear = currentYear - (currentAge - 9);
                endYear = startYear + 1;
            }
            else if (gradeNumber >= 7 && gradeNumber <= 10)
            {
                startYear = currentYear - (currentAge - 12);
                endYear = startYear + 1;
            }
            else if (gradeNumber == 11 || gradeNumber == 12)
            {
                startYear = currentYear - (currentAge - 17);
                endYear = startYear + 1;
            }
            else
            {
                return "Invalid grade number.";
            }

            return "SY " + startYear + " - " + endYear;
        }
    }

    // All data calculators
    public class Calculator
    {
        // Height Calculator
        public static float CalculateHeight(int age, int sex)
        {
            if (age < 1 || age > 80)
            {
                throw new ArgumentException("Age must be between 1 and 80");
            }

            if (sex != 0 && sex != 1)
            {
                throw new ArgumentException("Sex must be 'Male' or 'Female'");
            }

            float baseHeight = sex == 0 ? 163f : 152f; // Base height for male or female
            float ageMultiplier = sex == 0 ? 1.01f : 1.015f; // Multiplier based on sex
            float height = baseHeight + (age - 20) * ageMultiplier; // Linear increase from age 20 onwards

            return height;
        }

        // Weight Calculator
        public static float CalculateWeight(int age, int sex)
        {
            if (age < 1 || age > 80)
            {
                throw new ArgumentException("Age must be between 1 and 80");
            }

            if (sex != 0 && sex != 1)
            {
                throw new ArgumentException("Sex must be 'Male' or 'Female'");
            }

            float baseWeight = sex == 0 ? 53f : 46f; // Base weight for male or female
            float ageMultiplier = sex == 0 ? 1.01f : 1.015f; // Multiplier based on sex
            float weight = baseWeight + (age - 20) * ageMultiplier; // Linear increase from age 20 onwards

            return weight;
        }

        // Age Calculator
        public static int CalculateAge(DateTime birthdate, DateTime today)
        {
            if (birthdate > today)
            {
                throw new ArgumentException("Birthdate cannot be in the future");
            }

            int age = today.Year - birthdate.Year;

            if (birthdate.Month > today.Month || (birthdate.Month == today.Month && birthdate.Day > today.Day))
            {
                age--;
            }

            return age;
        }
    }

    // GLOBAL CLASSES:
    // Player Class
    public class Player
    {
        // Basic Information
        public string MfiName { get; set; }
        public int Budget { get; set; }

        // Gameplay Statistics
        public int TotalLoansCompleted { get; set; }
        public int TotalAmountEarned { get; set; }
        public int TotalLevelsCompleted { get; set; }
    }

    // Customer Class
    public class Customer
    {
        // Personal details
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public System.DateTime Bday { get; set; }
        public int Age { get; set; }

        // Loan Details
        public int LoanAmount { get; set; }
        public int AmountEarned { get; set; }
        public string Business { get; set; }
        public Identification IDUsed { get; set; }
        public string BusAddress { get; set; }
        public string HomeAddress { get; set; }
        public string ContactInfo { get; set; }
        public bool Approved { get; set; }
        public bool Paid { get; set; }

        // Appearance
        public int Clothes { get; set; }
        public int ClothesColor { get; set; }
        public int SkinType { get; set; }
        public int BodyColor { get; set; }
        public int EyeColor { get; set; }
        public int Eyes { get; set; }
        public int Nose { get; set; }
        public int Mouth { get; set; }
        public int Sex { get; set; }
        public int HairColor { get; set; }
        public int Brows { get; set; }
        public int Bangs { get; set; }
        public int Hair { get; set; }
        public int HairExtension { get; set; }

        // Probabilities
        public bool Real { get; set; }
        public bool Valid { get; set; }
        public float Rate { get; set; }
        public float Frequency { get; set; }
        public float StopChance { get; set; }
    }

    // Identification Class
    public class Identification
    {
        // Base Information
        public int IDTypeNumber { get; set; }
        public string IDType { get; set; }

        // ID Picture Appearance
        public int IDClothes { get; set; }
        public int IDClothesColor { get; set; }
        public int IDSkinType { get; set; }
        public int IDBodyColor { get; set; }
        public int IDEyeColor { get; set; }
        public int IDEyes { get; set; }
        public int IDNose { get; set; }
        public int IDMouth { get; set; }
        public int IDHairColor { get; set; }
        public int IDBrows { get; set; }
        public int IDBangs { get; set; }
        public int IDHair { get; set; }
        public int IDHairExtension { get; set; }

        // Common Details
        public string IDFirstName { get; set; }
        public string IDLastName { get; set; }
        public string IDNationality { get; set; }
        public int IDSex { get; set; }
        public string IDCivilStatus { get; set; }
        public string IDAddress { get; set; }
        public string IDBirthPlace { get; set; }
        public string IDNumber { get; set; }
        public System.DateTime IDBday { get; set; }
        public int IDAge { get; set; }
        public System.DateTime IDIssueDate { get; set; }
        public System.DateTime IDExpireDate { get; set; }
        public string IDContactNumber { get; set; }

        // Driver's License Details
        public float Weight { get; set; }
        public float Height { get; set; }
        public string BloodType { get; set; }
        public int Restriction { get; set; }
        public string Conditions { get; set; }

        // Senior ID Details
        public string EmergencyContactName { get; set; }
        public string EmergencyContactNumber { get; set; }

        // Barangay ID Details
        public string Barangay { get; set; }
        public string PrecinctNumber { get; set; }

        // Passport Details
        public string PassportType { get; set; }
        public string CountryCode { get; set; }

        // School ID Details
        public string SchoolName { get; set; }
        public string SchoolCity { get; set; }
        public int SchoolGrade { get; set; }
        public string SchoolSection { get; set; }
        public string SchoolAdviser { get; set; }
        public string SchoolYear { get; set; }

        // Bank Slip Details
        public string BankBranch { get; set; }
        public int DepositNumber { get; set; }
        public int DepositAmount { get; set; }
    }

    // GLOBAL FUNCTIONS:
    // New Player Generation Function
    public static Player NewPlayer(string mfi, int budget)
    {
        Player player = new Player();

        // Input basic player information
        player.MfiName = mfi;
        player.Budget = budget;

        // Ensure that all gameplay statistics are set to 0
        player.TotalLoansCompleted = 0;
        player.TotalAmountEarned = 0;
        player.TotalLevelsCompleted = 0;

        return player;
    }

    // New Customers Generation Function
    public static List<Customer> GenerateCustomers(int number, int minLoan, int maxLoan, float chanceForReal, float chanceForValid)
    {
        List<Customer> customers = new List<Customer>();

        // Define date information
        System.DateTime today = System.DateTime.Now;

        for (int i = 0; i < number; i++)
        {
            Customer customer = new Customer();

            // Generate Personal Details
            customer.FirstName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
            customer.LastName = lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
            customer.Bday = RandomDateGenerator.GenerateRandomDate(today.AddYears(-80), today.AddYears(-20));
            customer.Age = Calculator.CalculateAge(customer.Bday, today);

            // Generate Loan Details
            customer.LoanAmount = UnityEngine.Random.Range(minLoan, maxLoan);
            customer.AmountEarned = customer.LoanAmount;
            customer.Business = businesses[UnityEngine.Random.Range(0, businesses.Length)];
            customer.IDUsed = NewID();
            customer.BusAddress = busAddresses[UnityEngine.Random.Range(0, busAddresses.Length)];
            customer.HomeAddress = homeAddresses[UnityEngine.Random.Range(0, homeAddresses.Length)];
            customer.ContactInfo = RandomNumberGenerator.GenerateRandomPhoneNumber(10);
            customer.Approved = false;
            customer.Paid = false;

            // Generate customer appearance
            customer.Clothes = UnityEngine.Random.Range(0, 5);
            customer.ClothesColor = UnityEngine.Random.Range(0, 5);
            customer.SkinType = UnityEngine.Random.Range(0, 2);
            customer.BodyColor = UnityEngine.Random.Range(0, 5);
            customer.EyeColor = UnityEngine.Random.Range(0, 5);
            customer.Eyes = UnityEngine.Random.Range(0, 20);
            customer.Nose = UnityEngine.Random.Range(0, 20);
            customer.Mouth = UnityEngine.Random.Range(0, 20);

            customer.Sex = UnityEngine.Random.Range(0, 2);
            customer.HairColor = UnityEngine.Random.Range(0, 5);

            // Default Hair Color has 3 extra Brows
            if (customer.HairColor != 0)
            {
                customer.Brows = UnityEngine.Random.Range(0, 18);
                customer.Bangs = UnityEngine.Random.Range(0, 6);
                customer.Hair = UnityEngine.Random.Range(0, 6);
                customer.HairExtension = UnityEngine.Random.Range(0, 6);
            }
            else
            {
                customer.Brows = UnityEngine.Random.Range(0, 21);
                customer.Bangs = UnityEngine.Random.Range(0, 6);
                customer.Hair = UnityEngine.Random.Range(0, 6);
                customer.HairExtension = UnityEngine.Random.Range(0, 6);
            }

            // Ensure that customers aged 60+ actually look old: 1, 2, 3, 4, 6, 7, 8, 10, 13, 14, 15, 19, 20
            if (customer.Age >= 60)
            {
                int[] oldEyes = { 0, 1, 2, 3, 5, 6, 7, 9, 12, 13, 14, 18, 19 };
                int[] oldMouths = { 3, 4, 13, 14, 15, 16, 17 };

                customer.Eyes = oldEyes[UnityEngine.Random.Range(0, oldEyes.Length)];
                customer.Mouth = oldMouths[UnityEngine.Random.Range(0, oldMouths.Length)];
            }

            // Generate Probabilities
            customer.Real = UnityEngine.Random.value < chanceForReal;
            customer.Valid = UnityEngine.Random.value < chanceForValid;
            customer.Rate = UnityEngine.Random.Range(1, 3); // Fast[1], Slow[2], Erratic[3]
            customer.Frequency = UnityEngine.Random.Range(1, 3); // Early[1], Late[2], Steady[3]
            customer.StopChance = 0f;

            //Generate Common ID details
            customer.IDUsed.IDFirstName = customer.FirstName;
            customer.IDUsed.IDLastName = customer.LastName;
            customer.IDUsed.IDNationality = "Filipino";
            customer.IDUsed.IDSex = customer.Sex;
            customer.IDUsed.IDCivilStatus = idCivilStatus[UnityEngine.Random.Range(0, idCivilStatus.Length)];
            customer.IDUsed.IDAddress = customer.HomeAddress;
            customer.IDUsed.IDBirthPlace = idBirthPlace[UnityEngine.Random.Range(0, idBirthPlace.Length)];
            customer.IDUsed.IDBday = customer.Bday;
            customer.IDUsed.IDAge = customer.Age;
            customer.IDUsed.IDIssueDate = RandomDateGenerator.GenerateRandomDate(today.AddYears(-1), today.AddYears(-4));
            customer.IDUsed.IDExpireDate = customer.IDUsed.IDIssueDate.AddYears(5);
            customer.IDUsed.IDContactNumber = customer.ContactInfo;

            // Effects of Valid/Invalid
            if (customer.Valid)
            {
                // Create Valid ID
                customer.IDUsed.IDTypeNumber = idTypesValid[UnityEngine.Random.Range(0, idTypesValid.Length)];

                // Adjust for testing: 0 - "Driver's License", 1 - "Senior Citizen ID", 2 - "National ID", 3 - "Barangay ID", 4 - "Passport", 5 - "Elementary ID", 6 - "High School ID" 
                //customer.IDUsed.IDTypeNumber = 0;

                // Ensure only Seniors can have Senior IDs
                while (customer.Age < 60 && customer.IDUsed.IDTypeNumber == 1)
                {
                    customer.IDUsed.IDTypeNumber = idTypesValid[UnityEngine.Random.Range(0, idTypesValid.Length)];
                }

                // Create Valid ID Details based on type
                switch (customer.IDUsed.IDTypeNumber)
                {
                    // Driver's License Details
                    case 0:
                        customer.IDUsed.IDType = "Driver's License";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomLicenseNumber();
                        customer.IDUsed.Weight = Calculator.CalculateWeight(customer.IDUsed.IDAge, customer.IDUsed.IDSex);
                        customer.IDUsed.Height = Calculator.CalculateHeight(customer.IDUsed.IDAge, customer.IDUsed.IDSex);
                        customer.IDUsed.BloodType = idBloodTypes[UnityEngine.Random.Range(0, idBloodTypes.Length)];
                        customer.IDUsed.Restriction = UnityEngine.Random.Range(1, 9);
                        customer.IDUsed.Conditions = "NONE";
                        break;

                    // Senior ID Details
                    case 1:
                        customer.IDUsed.IDType = "Senior Citizen ID";
                        customer.IDUsed.IDNumber = UnityEngine.Random.Range(1000, 9999).ToString();
                        var emergencyName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
                        var emergencyNumber = RandomNumberGenerator.GenerateRandomPhoneNumber(10);

                        // Ensure that the emergency contact name and emergency contact number are different
                        while (customer.FirstName == emergencyName)
                        {
                            emergencyName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
                        }
                        while (customer.ContactInfo == emergencyNumber)
                        {
                            emergencyNumber = RandomNumberGenerator.GenerateRandomPhoneNumber(10);
                        }
                        customer.IDUsed.EmergencyContactName = emergencyName + " " + customer.LastName;
                        customer.IDUsed.EmergencyContactNumber = emergencyNumber;
                        break;

                    // National ID Details
                    case 2:
                        customer.IDUsed.IDType = "National ID";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomNationalIDNumber();
                        break;

                    // Barangay ID Details
                    case 3:
                        customer.IDUsed.IDType = "Barangay ID";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomBarangayIDNumber();
                        customer.IDUsed.Barangay = idBarangays[UnityEngine.Random.Range(0, idBarangays.Length)];
                        customer.IDUsed.PrecinctNumber = RandomNumberGenerator.GenerateRandomPrecinctNumber();
                        break;

                    // Passport Details
                    case 4:
                        customer.IDUsed.IDType = "Passport";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomPassportNumber();
                        customer.IDUsed.PassportType = "P";
                        customer.IDUsed.CountryCode = "PHL";
                        break;

                }
            }

            else
            {
                customer.StopChance = 0.1f;

                // Create Invalid ID
                customer.IDUsed.IDTypeNumber = idTypesInvalid[UnityEngine.Random.Range(0, idTypesInvalid.Length)];

                // Adjust for testing: 0 - "Driver's License", 1 - "Senior Citizen ID", 2 - "National ID", 3 - "Barangay ID", 4 - "Passport", 5 - "Elementary ID", 6 - "High School ID" 
                //customer.IDUsed.IDTypeNumber = 0;

                switch (customer.IDUsed.IDTypeNumber)
                {
                    // Driver's License Details
                    case 0:
                        customer.IDUsed.IDType = "Driver's License";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomLicenseNumber();
                        customer.IDUsed.Weight = Calculator.CalculateWeight(customer.IDUsed.IDAge, customer.IDUsed.IDSex);
                        customer.IDUsed.Height = Calculator.CalculateHeight(customer.IDUsed.IDAge, customer.IDUsed.IDSex);
                        customer.IDUsed.BloodType = idBloodTypes[UnityEngine.Random.Range(0, idBloodTypes.Length)];
                        customer.IDUsed.Restriction = UnityEngine.Random.Range(1, 9);
                        customer.IDUsed.Conditions = "NONE";
                        break;

                    // Senior ID Details
                    case 1:
                        // Ensure that the customer is actually a Senior
                        customer.IDUsed.IDBday = RandomDateGenerator.GenerateRandomDate(today.AddYears(-80), today.AddYears(-60));
                        customer.IDUsed.IDAge = Calculator.CalculateAge(customer.IDUsed.IDBday, today);

                        // Complete the rest of the details
                        customer.IDUsed.IDType = "Senior Citizen ID";
                        customer.IDUsed.IDNumber = UnityEngine.Random.Range(1000, 9999).ToString();
                        var emergencyName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
                        var emergencyNumber = RandomNumberGenerator.GenerateRandomPhoneNumber(10);
                        // Ensure that the emergency contact name and emergency contact number are different
                        while (customer.FirstName == emergencyName)
                        {
                            emergencyName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)];
                        }
                        while (customer.ContactInfo == emergencyNumber)
                        {
                            emergencyNumber = RandomNumberGenerator.GenerateRandomPhoneNumber(10);
                        }
                        customer.IDUsed.EmergencyContactName = emergencyName + " " + customer.LastName;
                        customer.IDUsed.EmergencyContactNumber = emergencyNumber;
                        break;

                    // National ID Details
                    case 2:
                        customer.IDUsed.IDType = "National ID";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomNationalIDNumber();
                        break;

                    // Barangay ID Details
                    case 3:
                        customer.IDUsed.IDType = "Barangay ID";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomBarangayIDNumber();
                        customer.IDUsed.Barangay = idBarangays[UnityEngine.Random.Range(0, idBarangays.Length)];
                        customer.IDUsed.PrecinctNumber = RandomNumberGenerator.GenerateRandomPrecinctNumber();
                        break;

                    // Passport Details
                    case 4:
                        customer.IDUsed.IDType = "Passport";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomPassportNumber();
                        customer.IDUsed.PassportType = "P";
                        customer.IDUsed.CountryCode = "PHL";
                        break;

                    // Elementary ID Details
                    case 5:
                        customer.IDUsed.IDType = "Elementary School ID";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomNumber(4).ToString();
                        customer.IDUsed.SchoolName = idElemSchools[UnityEngine.Random.Range(0, idElemSchools.Length)];
                        customer.IDUsed.SchoolCity = idSchoolCities[UnityEngine.Random.Range(0, idSchoolCities.Length)];
                        customer.IDUsed.SchoolGrade = UnityEngine.Random.Range(1, 6);
                        customer.IDUsed.SchoolSection = RandomNumberGenerator.GenerateSection(customer.IDUsed.SchoolGrade);
                        // Ensure that the adviser's name is different
                        var elemAdviserName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)] + " " + lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
                        while (customer.FirstName + " " + customer.LastName == elemAdviserName)
                        {
                            elemAdviserName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)] + " " + lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
                        }
                        customer.IDUsed.SchoolAdviser = elemAdviserName;
                        customer.IDUsed.SchoolYear = RandomNumberGenerator.GenerateSchoolYear(today.Year, customer.IDUsed.SchoolGrade, customer.IDUsed.IDAge);
                        break;

                    // High School ID Details
                    case 6:
                        customer.IDUsed.IDType = "High School ID";
                        customer.IDUsed.IDNumber = RandomNumberGenerator.GenerateRandomNumber(4).ToString();
                        customer.IDUsed.SchoolName = idHighSchools[UnityEngine.Random.Range(0, idHighSchools.Length)];
                        customer.IDUsed.SchoolCity = idSchoolCities[UnityEngine.Random.Range(0, idSchoolCities.Length)];
                        customer.IDUsed.SchoolGrade = UnityEngine.Random.Range(7, 12);
                        customer.IDUsed.SchoolSection = RandomNumberGenerator.GenerateSection(customer.IDUsed.SchoolGrade);
                        // Ensure that the adviser's name is different
                        var HSAdviserName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)] + " " + lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
                        while (customer.FirstName + " " + customer.LastName == HSAdviserName)
                        {
                            HSAdviserName = firstNames[UnityEngine.Random.Range(0, firstNames.Length)] + " " + lastNames[UnityEngine.Random.Range(0, lastNames.Length)];
                        }
                        customer.IDUsed.SchoolAdviser = HSAdviserName;
                        customer.IDUsed.SchoolYear = RandomNumberGenerator.GenerateSchoolYear(today.Year, customer.IDUsed.SchoolGrade, customer.IDUsed.IDAge);
                        break;

                    // Bank Slip Details
                    case 7:
                        customer.IDUsed.IDType = "Bank Slip";
                        customer.IDUsed.BankBranch = idBankBranches[UnityEngine.Random.Range(0, idBankBranches.Length)];
                        customer.IDUsed.DepositNumber = UnityEngine.Random.Range(100000000, 999999999);
                        customer.IDUsed.DepositAmount = UnityEngine.Random.Range(10000, 100000); ;
                        break;
                }
            }

            // Effects of Real/Fake
            if (customer.Real)
            {
                // Implement Real ID Details

                // Set ID picture to the same values as the real customer
                customer.IDUsed.IDClothes = customer.Clothes;
                customer.IDUsed.IDClothesColor = customer.ClothesColor;
                customer.IDUsed.IDSkinType = customer.SkinType;
                customer.IDUsed.IDBodyColor = customer.BodyColor;
                customer.IDUsed.IDEyeColor = customer.EyeColor;
                customer.IDUsed.IDEyes = customer.Eyes;
                customer.IDUsed.IDNose = customer.Nose;
                customer.IDUsed.IDMouth = customer.Mouth;
                customer.IDUsed.IDHairColor = customer.HairColor;
                customer.IDUsed.IDBrows = customer.Brows;
                customer.IDUsed.IDBangs = customer.Bangs;
                customer.IDUsed.IDHair = customer.Hair;
                customer.IDUsed.IDHairExtension = customer.HairExtension;
            }

            else
            {
                customer.StopChance = 0.5f;
                // Implement Fake ID Details
            }

            customers.Add(customer);
        }

        return customers;
    }

    // New ID Generation Function
    public static Identification NewID()
    {
        Identification id = new Identification();

        return id;
    }

    // Pause Game Function
    public static void PauseGame(bool truePause)
    {
        if (truePause)
        {
            isGamePaused = true;
        }

        Time.timeScale = 0f; // Freeze time
        Debug.Log("Game is paused: " + Time.timeScale.ToString());

        // Disable other interactive elements
    }

    // Unpause Game Function
    public static void UnpauseGame(bool truePause)
    {
        if (truePause)
        {
            isGamePaused = false;
        }

        Time.timeScale = 1f; // Resume time
        Debug.Log("Game is not paused: " + Time.timeScale.ToString());

        // Re-enable interactive elements
    }

    // Speed Game Function
    public static void SpeedGame(float scale)
    {
        if (Time.timeScale < 99f && Time.timeScale != 0f)
        {
            Time.timeScale += scale * Time.deltaTime;
            // Debug.Log("TimeScale: " + Time.timeScale.ToString());
        }
        else if (Time.timeScale != 0f)
        {
            Time.timeScale = 99f;
            // Debug.Log("TimeScale: " + Time.timeScale.ToString());
        }
    }

    // Reset Speed Game Function
    public static void ResetSpeedGame()
    {
        if (Time.timeScale != 0f)
        {
            Time.timeScale = 1;
        }
    }

    // Check Pause Status Function
    public static bool IsGamePaused()
    {
        return isGamePaused;
    }

    // Reset Global Data Function
    public static void ResetGlobalData()
    {
        PlayerData = null;
        CustomersData = new List<Customer>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
