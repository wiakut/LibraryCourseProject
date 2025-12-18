using LibrarySystem.Domain.Entities;
using LibrarySystem.Domain.Enums;
using LibrarySystem.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
namespace LibrarySystem.Infrastructure.Identity;
public static class IdentitySeedData
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { UserRole.Admin, UserRole.Reader };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }
    public static async Task SeedAdminUserAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        const string adminEmail = "admin@library.com";
        const string adminPassword = "Admin@123";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new IdentityUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, UserRole.Admin);
            }
        }
    }
    public static async Task SeedTestReaderUsersAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        const string testReaderEmail = "reader@library.com";
        const string testReaderPassword = "Reader@123";
        var defaultCategory = await context.ReaderCategories
            .FirstOrDefaultAsync(rc => rc.Name == "Standard");
        if (defaultCategory == null)
        {
            defaultCategory = await context.ReaderCategories
                .FirstOrDefaultAsync(rc => rc.Name == "Basic");
            if (defaultCategory == null)
            {
                defaultCategory = await context.ReaderCategories.FirstOrDefaultAsync();
                if (defaultCategory == null)
                {
                    // Categories should be seeded before this method is called
                    // If no categories exist, we cannot create a reader
                    return;
                }
            }
        }
        var user = await userManager.FindByEmailAsync(testReaderEmail);
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = "reader",
                Email = testReaderEmail,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(user, testReaderPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, UserRole.Reader);
                var reader = new Reader
                {
                    Id = Guid.NewGuid(),
                    Name = "Test Reader",
                    Address = "123 Test Street",
                    Phone = "+1-555-0000",
                    ReaderCategoryId = defaultCategory.Id,
                    UserId = user.Id
                };
                context.Readers.Add(reader);
                await context.SaveChangesAsync();
            }
        }
    }
    public static async Task SeedReaderCategoriesAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("IdentitySeedData");
        
        var expectedCategories = new[]
        {
            new { Name = "Basic", DiscountPercentage = 0m },
            new { Name = "Standard", DiscountPercentage = 5m },
            new { Name = "Premium", DiscountPercentage = 10m },
            new { Name = "Gold", DiscountPercentage = 15m },
            new { Name = "VIP", DiscountPercentage = 20m },
            new { Name = "Student", DiscountPercentage = 12m },
            new { Name = "Senior", DiscountPercentage = 15m },
            new { Name = "Corporate", DiscountPercentage = 25m }
        };

        var existingCategories = await context.ReaderCategories.ToListAsync();
        var categoriesToAdd = new List<ReaderCategory>();

        foreach (var expectedCategory in expectedCategories)
        {
            if (!existingCategories.Any(c => c.Name == expectedCategory.Name))
            {
                categoriesToAdd.Add(new ReaderCategory
                {
                    Id = Guid.NewGuid(),
                    Name = expectedCategory.Name,
                    DiscountPercentage = expectedCategory.DiscountPercentage
                });
            }
        }

        if (categoriesToAdd.Any())
        {
            await context.ReaderCategories.AddRangeAsync(categoriesToAdd);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} reader categories", categoriesToAdd.Count);
        }
        else
        {
            logger.LogInformation("All reader categories already exist");
        }
    }

    public static async Task SeedDemoDataAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var loggerFactory = scope.ServiceProvider.GetRequiredService<ILoggerFactory>();
        var logger = loggerFactory.CreateLogger("IdentitySeedData");
        logger.LogInformation("Starting demo data seeding...");
        var random = new Random();
        var allCategories = await context.ReaderCategories.ToListAsync();
        logger.LogInformation("Seeding books...");
        if (!await context.Books.AnyAsync())
        {
            var genres = new[]
            {
                "Fiction", "Science Fiction", "Fantasy", "Mystery", "Thriller", "Romance",
                "Historical Fiction", "Literary Fiction", "Horror", "Adventure", "Crime",
                "Non-Fiction", "History", "Biography", "Memoir", "Science", "Technology",
                "Philosophy", "Psychology", "Business", "Economics", "Education", "Art",
                "Music", "Poetry", "Drama", "Young Adult", "Children's Literature",
                "Health & Fitness", "Cookbooks", "Travel", "Self-Help", "Religion"
            };
            var firstNames = new[]
            {
                "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph",
                "Charles", "Thomas", "Christopher", "Daniel", "Matthew", "Anthony", "Mark",
                "Donald", "Steven", "Paul", "Andrew", "Joshua", "Kenneth", "Kevin", "Brian",
                "George", "Timothy", "Ronald", "Edward", "Jason", "Jeffrey", "Ryan",
                "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan",
                "Jessica", "Sarah", "Karen", "Nancy", "Lisa", "Betty", "Margaret", "Sandra",
                "Ashley", "Kimberly", "Emily", "Donna", "Michelle", "Carol", "Amanda", "Dorothy"
            };
            var lastNames = new[]
            {
                "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
                "Rodriguez", "Martinez", "Hernandez", "Lopez", "Wilson", "Anderson", "Thomas",
                "Taylor", "Moore", "Jackson", "Martin", "Lee", "Thompson", "White", "Harris",
                "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker", "Young",
                "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
                "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell"
            };
            var bookTitlePrefixes = new[]
            {
                "The", "A", "An", "In", "On", "Of", "For", "Beyond", "Through", "Into",
                "Under", "Above", "Between", "Within", "Without", "Against", "Among"
            };
            var bookTitleWords = new[]
            {
                "Adventure", "Journey", "Secret", "Mystery", "Power", "Light", "Dark", "Fire",
                "Water", "Wind", "Earth", "Sky", "Star", "Moon", "Sun", "Heart", "Soul",
                "Mind", "Spirit", "Dream", "Night", "Day", "Time", "Space", "World", "Land",
                "Sea", "Mountain", "River", "Forest", "City", "Kingdom", "Empire", "War",
                "Peace", "Love", "Hate", "Fear", "Hope", "Truth", "Lie", "Life", "Death",
                "Legend", "Story", "Tale", "History", "Future", "Past", "Present", "Quest"
            };
            var books = new List<Book>();
            for (var i = 1; i <= 450; i++)
            {
                var genre = genres[random.Next(genres.Length)];
                var authorFirstName = firstNames[random.Next(firstNames.Length)];
                var authorLastName = lastNames[random.Next(lastNames.Length)];
                var author = $"{authorFirstName} {authorLastName}";
                string title;
                if (random.NextDouble() < 0.4)
                {
                    var prefix = bookTitlePrefixes[random.Next(bookTitlePrefixes.Length)];
                    var word1 = bookTitleWords[random.Next(bookTitleWords.Length)];
                    var word2 = bookTitleWords[random.Next(bookTitleWords.Length)];
                    title = $"{prefix} {word1} {word2}";
                }
                else if (random.NextDouble() < 0.7)
                {
                    var word1 = bookTitleWords[random.Next(bookTitleWords.Length)];
                    var word2 = bookTitleWords[random.Next(bookTitleWords.Length)];
                    title = $"{word1} of {word2}";
                }
                else
                {
                    var word1 = bookTitleWords[random.Next(bookTitleWords.Length)];
                    var word2 = bookTitleWords[random.Next(bookTitleWords.Length)];
                    title = $"{word1}: {word2}";
                }
                var baseRental = genre switch
                {
                    "Technology" or "Business" or "Science" => (decimal)random.Next(5, 16), // 5-15
                    "Fiction" or "Mystery" or "Romance" => (decimal)random.Next(2, 8), // 2-7
                    _ => (decimal)random.Next(3, 12) // 3-11
                };
                var pledge = baseRental * random.Next(8, 15); // 8-14x base rental
                var totalCount = random.Next(1, 8); // 1-7 copies for variety
                books.Add(new Book
                {
                    Id = Guid.NewGuid(),
                    Title = title,
                    Author = author,
                    Genre = genre,
                    PledgeValue = pledge,
                    BaseRentalCost = baseRental,
                    TotalCount = totalCount,
                    AvailableCount = totalCount, // Will be updated after rentals are created
                    InRentCount = 0
                });
            }
            await context.Books.AddRangeAsync(books);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} books successfully", books.Count);
        }
        var allBooks = await context.Books.ToListAsync();
        logger.LogInformation("Seeding readers...");
        var currentReaderCount = await context.Readers.CountAsync();
        var testReaderUserCheck = await userManager.FindByEmailAsync("reader@library.com");
        var hasDemoReadersNow = testReaderUserCheck != null
            ? currentReaderCount > 1  // More than just the test reader
            : currentReaderCount > 0; // Any readers if no test reader exists
        if (!hasDemoReadersNow)
        {
            var firstNames = new[]
            {
                "James", "John", "Robert", "Michael", "William", "David", "Richard", "Joseph",
                "Charles", "Thomas", "Christopher", "Daniel", "Matthew", "Anthony", "Mark",
                "Donald", "Steven", "Paul", "Andrew", "Joshua", "Kenneth", "Kevin", "Brian",
                "George", "Timothy", "Ronald", "Edward", "Jason", "Jeffrey", "Ryan",
                "Mary", "Patricia", "Jennifer", "Linda", "Elizabeth", "Barbara", "Susan",
                "Jessica", "Sarah", "Karen", "Nancy", "Lisa", "Betty", "Margaret", "Sandra",
                "Ashley", "Kimberly", "Emily", "Donna", "Michelle", "Carol", "Amanda", "Dorothy",
                "Melissa", "Deborah", "Stephanie", "Rebecca", "Sharon", "Laura", "Cynthia",
                "Kathleen", "Amy", "Angela", "Shirley", "Anna", "Brenda", "Pamela", "Emma",
                "Nicole", "Helen", "Samantha", "Katherine", "Christine", "Debra", "Rachel"
            };
            var lastNames = new[]
            {
                "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis",
                "Rodriguez", "Martinez", "Hernandez", "Lopez", "Wilson", "Anderson", "Thomas",
                "Taylor", "Moore", "Jackson", "Martin", "Lee", "Thompson", "White", "Harris",
                "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson", "Walker", "Young",
                "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
                "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell",
                "Carter", "Roberts", "Gomez", "Phillips", "Evans", "Turner", "Diaz", "Parker",
                "Cruz", "Edwards", "Collins", "Reyes", "Stewart", "Morris", "Morales", "Murphy"
            };
            var streetNames = new[]
            {
                "Main", "Park", "Oak", "Pine", "Elm", "Maple", "Cedar", "First", "Second",
                "Third", "Washington", "Lincoln", "Jefferson", "Madison", "Franklin", "Church",
                "Broadway", "Center", "Market", "High", "School", "River", "Lake", "Hill",
                "Valley", "Forest", "Sunset", "Sunrise", "Chestnut", "Walnut", "Birch",
                "Spring", "Summer", "Winter", "Autumn", "Green", "Red", "Blue", "King",
                "Queen", "State", "Union", "Liberty", "Freedom", "Peace", "Hope"
            };
            var streetTypes = new[] { "Street", "Avenue", "Road", "Drive", "Lane", "Court", "Place", "Way", "Boulevard", "Circle" };
            var readers = new List<Reader>();
            for (var i = 1; i <= 450; i++)
            {
                var firstName = firstNames[random.Next(firstNames.Length)];
                var lastName = lastNames[random.Next(lastNames.Length)];
                var fullName = $"{firstName} {lastName}";
                var email = $"reader{i}@demo.com";
                var userName = $"reader{i}";
                var identityUser = await userManager.FindByEmailAsync(email);
                if (identityUser == null)
                {
                    identityUser = new IdentityUser
                    {
                        UserName = userName,
                        Email = email,
                        EmailConfirmed = true
                    };
                    var createResult = await userManager.CreateAsync(identityUser, "Reader@123");
                    if (createResult.Succeeded)
                    {
                        await userManager.AddToRoleAsync(identityUser, UserRole.Reader);
                    }
                    else
                    {
                        continue; // Skip if user creation failed
                    }
                }
                var categoryRoll = random.NextDouble();
                ReaderCategory category;
                if (categoryRoll < 0.25)
                {
                    category = allCategories.FirstOrDefault(c => c.Name == "Basic") ?? allCategories.First();
                }
                else if (categoryRoll < 0.50)
                {
                    category = allCategories.FirstOrDefault(c => c.Name == "Standard") ?? allCategories.First();
                }
                else if (categoryRoll < 0.65)
                {
                    category = allCategories.FirstOrDefault(c => c.Name == "Premium") ?? allCategories.First();
                }
                else if (categoryRoll < 0.75)
                {
                    category = allCategories.FirstOrDefault(c => c.Name == "Gold") ?? allCategories.First();
                }
                else if (categoryRoll < 0.82)
                {
                    category = allCategories.FirstOrDefault(c => c.Name == "Student") ?? allCategories.First();
                }
                else if (categoryRoll < 0.89)
                {
                    category = allCategories.FirstOrDefault(c => c.Name == "Senior") ?? allCategories.First();
                }
                else if (categoryRoll < 0.95)
                {
                    category = allCategories.FirstOrDefault(c => c.Name == "VIP") ?? allCategories.First();
                }
                else
                {
                    category = allCategories.FirstOrDefault(c => c.Name == "Corporate") ?? allCategories.First();
                }
                var streetNumber = random.Next(1, 9999);
                var streetName = streetNames[random.Next(streetNames.Length)];
                var streetType = streetTypes[random.Next(streetTypes.Length)];
                var address = $"{streetNumber} {streetName} {streetType}";
                var areaCode = random.Next(200, 999);
                var exchange = random.Next(200, 999);
                var number = random.Next(1000, 9999);
                var phone = $"+1-{areaCode}-{exchange}-{number}";
                readers.Add(new Reader
                {
                    Id = Guid.NewGuid(),
                    Name = fullName,
                    Address = address,
                    Phone = phone,
                    ReaderCategoryId = category.Id,
                    UserId = identityUser.Id
                });
            }
            await context.Readers.AddRangeAsync(readers);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} readers successfully", readers.Count);
        }
        var allReaders = await context.Readers
            .Include(r => r.ReaderCategory)
            .ToListAsync();
        logger.LogInformation("Seeding rental transactions...");
        if (!await context.RentalTransactions.AnyAsync())
        {
            var rentals = new List<RentalTransaction>();
            var fines = new List<Fine>();
            var now = DateTime.UtcNow;
            const decimal dailyFineRate = 2m;
            for (var i = 0; i < 600; i++)
            {
                var book = allBooks[random.Next(allBooks.Count)];
                var reader = allReaders[random.Next(allReaders.Count)];
                var issueOffsetDays = random.Next(0, 180);
                var issueDate = now.AddDays(-issueOffsetDays);
                var rentalDays = random.Next(3, 28); // 3 - 27 days
                var expectedReturnDate = issueDate.AddDays(rentalDays);
                var baseRentalCost = book.BaseRentalCost * rentalDays;
                var discountAmount = baseRentalCost * (reader.ReaderCategory.DiscountPercentage / 100m);
                var finalRentalCost = baseRentalCost - discountAmount;
                var pledgeAmount = book.PledgeValue;
                var isReturned = random.NextDouble() < 0.65;
                DateTime? actualReturnDate = null;
                decimal fineAmount = 0m;
                decimal refundAmount = 0m;
                var status = RentalStatus.Active;
                if (isReturned)
                {
                    var extraDays = random.Next(-3, 20); // can be early or late
                    actualReturnDate = expectedReturnDate.AddDays(extraDays);
                    if (actualReturnDate < issueDate)
                    {
                        actualReturnDate = issueDate;
                    }
                    if (actualReturnDate > now)
                    {
                        actualReturnDate = now.AddDays(-random.Next(0, 5)); // Ensure it's in the past
                    }
                    var overdueDays = (actualReturnDate.Value.Date - expectedReturnDate.Date).Days;
                    if (overdueDays < 0)
                    {
                        overdueDays = 0;
                    }
                    var overdueFine = overdueDays * dailyFineRate;
                    decimal damageCost = 0m;
                    if (random.NextDouble() < 0.20)
                    {
                        damageCost = (decimal)random.Next(5, 61); // 5 - 60
                    }
                    fineAmount = overdueFine + damageCost;
                    refundAmount = Math.Max(0, pledgeAmount - fineAmount); // Ensure non-negative
                    status = RentalStatus.Returned;
                }
                var rental = new RentalTransaction
                {
                    Id = Guid.NewGuid(),
                    BookId = book.Id,
                    ReaderId = reader.Id,
                    IssueDate = issueDate,
                    ExpectedReturnDate = expectedReturnDate,
                    ActualReturnDate = actualReturnDate,
                    Status = status,
                    BaseRentalCost = baseRentalCost,
                    FinalRentalCost = finalRentalCost,
                    PledgeAmount = pledgeAmount,
                    RefundAmount = refundAmount,
                    FineAmount = fineAmount
                };
                rentals.Add(rental);
                if (fineAmount > 0 && isReturned)
                {
                    var overdueDays = (actualReturnDate!.Value.Date - expectedReturnDate.Date).Days;
                    if (overdueDays < 0) overdueDays = 0;
                    var overdueFine = overdueDays * dailyFineRate;
                    var hasDamage = fineAmount > overdueFine;
                    var fine = new Fine
                    {
                        Id = Guid.NewGuid(),
                        RentalTransactionId = rental.Id,
                        Amount = fineAmount,
                        Reason = overdueFine > 0 && hasDamage
                            ? "Overdue and damage"
                            : overdueFine > 0
                                ? "Overdue"
                                : "Damage",
                        CreatedDate = actualReturnDate.Value
                    };
                    fines.Add(fine);
                }
            }
            await context.RentalTransactions.AddRangeAsync(rentals);
            await context.Fines.AddRangeAsync(fines);
            await context.SaveChangesAsync();
            foreach (var book in allBooks)
            {
                var activeRentalsCount = rentals.Count(r => r.BookId == book.Id && r.Status == RentalStatus.Active);
                book.InRentCount = activeRentalsCount;
                book.AvailableCount = Math.Max(0, book.TotalCount - activeRentalsCount);
            }
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {RentalCount} rental transactions and {FineCount} fines successfully", rentals.Count, fines.Count);
        }
        logger.LogInformation("Seeding rental requests...");
        if (!await context.RentalRequests.AnyAsync())
        {
            var adminUser = await userManager.FindByEmailAsync("admin@library.com");
            if (adminUser == null) return;
            var rentalRequests = new List<RentalRequest>();
            var now = DateTime.UtcNow;
            var targetRequests = 450;
            var requestsCreated = 0;
            var bookIndex = 0;
            while (requestsCreated < targetRequests && bookIndex < allBooks.Count)
            {
                var book = allBooks[bookIndex];
                var requestsForBook = random.Next(1, 5);
                for (var i = 0; i < requestsForBook && requestsCreated < targetRequests; i++)
                {
                    var reader = allReaders[random.Next(allReaders.Count)];
                    var requestDate = now.AddDays(-random.Next(0, 90)); // Requests from last 90 days
                    var statusRoll = random.NextDouble();
                    RentalRequestStatus status;
                    DateTime? processedDate = null;
                    string? denialReason = null;
                    DateTime? expectedReturnDate = null;
                    if (statusRoll < 0.35)
                    {
                        status = RentalRequestStatus.Pending;
                    }
                    else if (statusRoll < 0.70)
                    {
                        status = RentalRequestStatus.Approved;
                        processedDate = requestDate.AddDays(random.Next(1, 7));
                        expectedReturnDate = processedDate.Value.AddDays(random.Next(7, 28));
                    }
                    else
                    {
                        status = RentalRequestStatus.Denied;
                        processedDate = requestDate.AddDays(random.Next(1, 7));
                        var reasons = new[]
                        {
                            "Book is currently unavailable",
                            "Reader has reached maximum rental limit",
                            "Book is reserved for another reader",
                            "Temporary unavailability",
                            "Maintenance required",
                            "Insufficient copies available",
                            "Reader account has outstanding fines",
                            "Book is being repaired"
                        };
                        denialReason = reasons[random.Next(reasons.Length)];
                    }
                    rentalRequests.Add(new RentalRequest
                    {
                        Id = Guid.NewGuid(),
                        BookId = book.Id,
                        ReaderId = reader.Id,
                        RequestDate = requestDate,
                        Status = status,
                        ExpectedReturnDate = expectedReturnDate,
                        DenialReason = denialReason,
                        ProcessedDate = processedDate,
                        ProcessedByUserId = adminUser.Id
                    });
                    requestsCreated++;
                }
                bookIndex++;
            }
            await context.RentalRequests.AddRangeAsync(rentalRequests);
            await context.SaveChangesAsync();
            logger.LogInformation("Seeded {Count} rental requests successfully", rentalRequests.Count);
        }
        logger.LogInformation("Demo data seeding completed successfully!");
    }
}