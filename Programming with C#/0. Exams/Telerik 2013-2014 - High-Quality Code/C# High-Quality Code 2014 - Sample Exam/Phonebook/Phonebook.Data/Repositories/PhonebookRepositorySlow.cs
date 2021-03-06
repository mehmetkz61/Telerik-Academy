namespace Phonebook.Data.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Phonebook.Data.Contracts;
    using Phonebook.Models;
    using Phonebook.Models.Contracts;

    public class PhonebookRepositorySlow : IPhonebookRepository, IDeletablePhonebookRepository
    {
        private const int MinListEntriesCount = 1;
        private const int MaxListEntriesCount = 20;

        private readonly List<IPhoneEntry> entries = new List<IPhoneEntry>();

        public bool AddPhone(string name, IEnumerable<string> phoneNumbers)
        {
            var old = from e in this.entries
                      where e.Name.ToLowerInvariant() == name.ToLowerInvariant()
                      select e;

            bool flag;
            if (old.Count() == 0)
            {
                var obj = new PhoneEntry(name);

                foreach (var num in phoneNumbers)
                {
                    obj.PhoneNumbers.Add(num);
                }

                this.entries.Add(obj);

                flag = true;
            }
            else if (old.Count() == 1)
            {
                IPhoneEntry obj2 = old.First();
                foreach (var num in phoneNumbers)
                {
                    obj2.PhoneNumbers.Add(num);
                }

                flag = false;
            }
            else
            {
                Console.WriteLine("Duplicated name in the phonebook found: " + name);
                return false;
            }

            return flag;
        }

        public bool DeletePhone(string phoneNumber)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public int ChangePhone(string oldPhoneNumber, string newPhoneNumber)
        {
            if (string.IsNullOrEmpty(oldPhoneNumber) || string.IsNullOrEmpty(newPhoneNumber))
            {
                throw new ArgumentException("Phone numbers cannot be null or empty.");
            }

            var matchedPhoneEntries = this.entries.Where(e => e.PhoneNumbers.Contains(oldPhoneNumber));
            var numberOfChangedPhoneEntries = matchedPhoneEntries.Count();

            foreach (var phoneEntry in matchedPhoneEntries)
            {
                phoneEntry.PhoneNumbers.Remove(oldPhoneNumber);
                phoneEntry.PhoneNumbers.Add(newPhoneNumber);
            }

            return numberOfChangedPhoneEntries;
        }

        public IEnumerable<IPhoneEntry> ListEntries(int startIndex, int count)
        {
            if (startIndex < 0 || startIndex + count > this.entries.Count)
            {
                throw new ArgumentOutOfRangeException("Invalid start index value. Start index is out of range.");
            }

            if (count < MinListEntriesCount || count > MaxListEntriesCount)
            {
                throw new ArgumentOutOfRangeException(
                    string.Format("Count must be in range [{0};{1}]", MinListEntriesCount, MaxListEntriesCount));
            }

            this.entries.Sort();
            var listedPhoneEntries = this.entries.Skip(startIndex).Take(count).ToList();
            return listedPhoneEntries;
        }
    }
}