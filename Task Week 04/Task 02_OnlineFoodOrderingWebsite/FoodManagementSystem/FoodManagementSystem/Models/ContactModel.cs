using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Threading.Tasks;
using System.Web;

namespace FoodManagementSystem.Models
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string SenderName { get; set; }
        public string EmailAddress { get; set; }
        public string SenderSubject { get; set; }
        public string SenderMessage { get; set; }
        public DateTime CreatedDate { get; set; }

        OnlineFoodOrderingSystemEntities db = new OnlineFoodOrderingSystemEntities();
        public async Task<int> ContactUS()
        {
            ContactMessage cm = new ContactMessage
            {
                Id = Id,
                SenderName = SenderName,
                EmailAddress = EmailAddress,
                SenderSubject = SenderSubject,
                SenderMessage = SenderMessage,
                CreatedDate = DateTime.Now
            };
            db.ContactMessages.Add(cm);
            await db.SaveChangesAsync();
            return 1;

        }
    }
}