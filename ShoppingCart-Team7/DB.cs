using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using ShoppingCart_Team7.Models;
using System.Diagnostics;

namespace ShoppingCart_Team7
{
    public class DB
    {
        private DBContext dbContext;

        public DB(DBContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Seed()
        {
            SeedUsers();
            SeedProducts();
            SeedProducts1();
            SeedPurchase();
            SeedReview();
            SeedSession();
            SeedCart();
        }

        public void SeedUsers()
        {
            HashAlgorithm sha = SHA256.Create();

            string[] usernames = { "charles", "jack", "jamie", "ann", "mark", "kevin", "denise", "eliza", "peter", "jane" };
            string[] firstName = { "Charles", "Jack", "Jamie", "Ann", "Mark", "Kevin", "Denise", "Eliza", "Peter", "Jane" };
            string[] lastName = { "Tan", "Lee", "Russell", "Lee", "Ng", "Leong", "Peeters", "Robins", "Ng", "Potter" };


            // password is same as username
            int counter = 0;
            foreach (string user in usernames)
            {
                string combination = user + user;
                byte[] hash = sha.ComputeHash(Encoding.UTF8.GetBytes(combination));

                dbContext.Add(new User
                {
                    FirstName = firstName[counter],
                    LastName = lastName[counter],
                    UserName = user,
                    PasswordHash = hash
                });

                counter++;
            }

            dbContext.SaveChanges();
        }

        public void SeedProducts()
        {
            string[] Title = { "No Longer Human", "Pride and Prejudice", "Dr Jekyll and Mr Hyde",
                                "Jane Eyre", "Julius Caesar: The Pelican Shakespeare", "1984",
                                "Edgar Allen Poe: Classic Stories", "The Great Gatsby"};
            float[] Price = { 39.99f, 8.5f, 10f, 13.99f, 17.5f, 3.99f, 19.95f, 11.5f };
            string[] Desc = {   "This story tells the poignant and fascinating story of a young man who is caught between the breakup of the traditions " +
                                    "of a northern Japanese aristocratic family and the impact of Western ideas.",
                                    "Pride and Prejudice is a novel of manners by Jane Austen, first published in 1813. The story follows the main character, " +
                                    "Elizabeth Bennet, as she deals with issues of manners, upbringing, morality, education, and marriage in the society of the " +
                                    "landed gentry of the British Regency.",
                                    "The story of respectable Dr Jekyll's strange association with 'damnable young man' Edward Hyde; the hunt through fog-bound London " +
                                    "for a killer; and the final revelation of Hyde's true identity is a chilling exploration of humanity's basest capacity for evil.",
                                    "A young governess finds romance, mystery, and heartbreak when she accepts a position in the home of the brooding and arrogant Mr. Rochester.",
                                    "The legendary Pelican Shakespeare series features authoritative and meticulously researched texts paired with scholarship by renowned Shakespeareans.",
                                    "While the totalitarianism that provoked George Orwell into writing Nineteen Eighty-Four seems to be passing into oblivion, his harrowing, cautionary tale " +
                                    "of a man trapped in a political nightmare has had the opposite fate, and its relevance and power to disturb our complacency seem to grow decade by decade.",
                                    "Classic stories by Edgar Allen Poe",
                                    "The story of the mysteriously wealthy Jay Gatsby and his love for the beautiful Daisy Buchanan, of lavish parties on Long Island."};

            string[] ImgSrc = { "/cf1.jpg", "/cf2.jpg", "/cf3.jpg", "/cf4.jpg", "/cf5.jpg", "/cf6.jpg", "/cf7.jpg", "/cf8.jpg" };

            int counter = 0;
            foreach (string book in Title)
            {
                dbContext.Add(new Product
                {
                    ProductName = book,
                    Price = Price[counter],
                    Description = Desc[counter],
                    ImageSrc = ImgSrc[counter],
                    Category = "Classic Fiction"
                });
                counter++;
            }
            dbContext.SaveChanges();
        }

        public void SeedProducts1()
        {
            string[] Title = { "Nineteen Minutes", "The Guest List", "Then She Was Gone", "Murder on the Orient Express",
                                "Magpie", "Flowers for Algernon", "The Stranger"};
            float[] Price = { 18.8f, 14.99f, 15f, 12.30f, 17.75f, 12.6f, 14.4f};
            string[] Desc = {   "Jodi Picoult, bestselling author of My Sister's Keeper and Small Great Things pens her most riveting book yet, with a startling and" +
                                " poignant story about the devastating aftermath of a small-town tragedy.",
                                "A gripping, twisty murder mystery thriller from the No.1 bestselling author of The Hunting Party.",
                                "Ten years on, Laurel has never given up hope of finding Ellie. And then she meets a charming and charismatic stranger who sweeps " +
                                "her off her feet. But what really takes her breath away is when she meets his nine-year-old daughter. Because his daughter is the " +
                                "image of Ellie. Now all those unanswered questions that have haunted Laurel come flooding back.",
                                "Just after midnight, a snowdrift stops the Orient Express in its tracks. The luxurious train is surprisingly full for the time of the year, " +
                                "but by the morning it is one passenger fewer. An American tycoon lies dead in his compartment, stabbed a dozen times, his door locked from the inside." +
                                "Isolated and with a killer in their midst, detective Hercule Poirot must identify the murderer – in case he or she decides to strike again.",
                                "Magpie is a tense, twisting, brilliantly written novel about mothers and children, envy and possession, and the dangers of getting everything you’ve ever dreamed of.",
                                "Algernon is a laboratory mouse who has undergone surgery to increase his intelligence. The story is told by a series of progress reports written by Charlie Gordon, the " +
                                "first human subject for the surgery, and it touches on ethical and moral themes such as the treatment of the mentally disabled.",
                                "Behind the intrigue, Camus explores what he termed \"the nakedness of man faced with the absurd\" and describes the condition of reckless " +
                                "alienation and spiritual exhaustion that characterized so much of twentieth-century life."};
            string[] ImgSrc = {"/t1.jpg", "/t2.jpg" , "/t3.jpg", "/t4.jpg", "/t5.jpg", "/t6.jpg", "/t7.jpg"};

            int counter = 0;
            foreach (string book in Title)
            {
                dbContext.Add(new Product
                {
                    ProductName = book,
                    Price = Price[counter],
                    Description = Desc[counter],
                    ImageSrc = ImgSrc[counter],
                    Category = "Thriller"
                });
                counter++;
            }
            dbContext.SaveChanges();
        }

        public void SeedPurchase()
        {
            User user1 = dbContext.Users.FirstOrDefault(x => x.UserName == "jack");
            Product product1 = dbContext.Products.FirstOrDefault(x => x.ProductName == "Jane Eyre");

            if (product1 != null)
            {
                Purchase purchase1 = new Purchase
                {
                    PurchaseDate = new DateTime(2021, 10, 1, 9, 0, 0, DateTimeKind.Local),
                    ActivationCode = Guid.NewGuid()
                };
                user1.Purchases.Add(purchase1);
                product1.Purchases.Add(purchase1);
            }

            User user2 = dbContext.Users.FirstOrDefault(x => x.UserName == "jack");
            Product product2 = dbContext.Products.FirstOrDefault(x => x.ProductName == "No Longer Human");

            if (product2 != null)
            {
                Purchase purchase2 = new Purchase
                {
                    PurchaseDate = new DateTime(2021, 10, 1, 9, 0, 0, DateTimeKind.Local),
                    ActivationCode = Guid.NewGuid()
                };
                user2.Purchases.Add(purchase2);
                product2.Purchases.Add(purchase2);
            }

            dbContext.SaveChanges();
        }

        public void SeedReview()
        {

            // User user = dbContext.Users.FirstOrDefault(x => x.UserName == "jack");
            // Purchase purchase = dbContext.Purchases.FirstOrDefault(x => x.UserId == user.Id);
            
            Product product1 = dbContext.Products.FirstOrDefault(x => x.ProductName == "Jane Eyre");

            if (product1 != null)
            {
                Review review1 = new Review()
                {
                    Comments = "Loved the book! Download was easy and efficient.",
                    Rating = 4,
                    ReviewDate = new DateTime(2021, 10, 15, 10, 0, 0),
                };
                product1.Reviews.Add(review1);
            }

            dbContext.SaveChanges();
        }

        public void SeedSession()
        {
            User jack = dbContext.Users.FirstOrDefault(x => x.UserName == "jack");
            if (jack != null)
            {
                Session session = new Session()
                {
                    SessionTime = DateTimeOffset.Now.ToUnixTimeSeconds(),
                    Valid = true
                };

                jack.Sessions.Add(session);
            }

            dbContext.SaveChanges();
        }

        public void SeedCart()
        {
            User user1 = dbContext.Users.FirstOrDefault(x => x.UserName == "jack");
            Product product1 = dbContext.Products.FirstOrDefault(x => x.ProductName == "1984");
            Product product2 = dbContext.Products.FirstOrDefault(x => x.ProductName == "Magpie");

            if (product1 != null)
            {
                Cart cart1 = new Cart
                {
                    Quantity = 1
                };

                user1.Carts.Add(cart1);
                product1.Carts.Add(cart1);

                Cart cart2 = new Cart
                {
                    Quantity = 2
                };

                user1.Carts.Add(cart2);
                product2.Carts.Add(cart2);
            }
            dbContext.SaveChanges();
        }
    }
}
