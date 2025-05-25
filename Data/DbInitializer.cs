using landlord_be.Models;

namespace landlord_be.Data
{
    public class DbInitializer
    {
        private readonly ILogger _logger;

        public DbInitializer(ILogger logger)
        {
            _logger = logger;
        }

        public void Initialize(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            DateTime time = DateTime.Now.ToUniversalTime();
            var users = new List<User>
            {
                new User { Name = "Иван Иванов", PasswordHash = "hash1", Token = "token1", RegisterDate = time, UpdateDate = time },
            new User { Name = "Петр Петров", PasswordHash = "hash2", Token = "token2", RegisterDate = time, UpdateDate = time },
            new User { Name = "Сергей Сергеев", PasswordHash = "hash3", Token = "token3", RegisterDate = time, UpdateDate = time },
            new User { Name = "Алексей Алексеев", PasswordHash = "hash4", Token = "token4", RegisterDate = time, UpdateDate = time },
            new User { Name = "Дмитрий Дмитриев", PasswordHash = "hash5", Token = "token5", RegisterDate = time, UpdateDate = time },
            new User { Name = "Анна Антонова", PasswordHash = "hash6", Token = "token6", RegisterDate = time, UpdateDate = time },
            new User { Name = "Мария Маркова", PasswordHash = "hash7", Token = "token7", RegisterDate = time, UpdateDate = time },
            new User { Name = "Елена Еленина", PasswordHash = "hash8", Token = "token8", RegisterDate = time, UpdateDate = time },
            new User { Name = "Олег Олегов", PasswordHash = "hash9", Token = "token9", RegisterDate = time, UpdateDate = time },
            new User { Name = "Татьяна Татьянова", PasswordHash = "hash10", Token = "token10", RegisterDate = time, UpdateDate = time },
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            // Create properties for each user
            var properties = new List<Property>
        {
            new Property { OwnerId = users[0].Id, Name = "Квартира в центре", Desc = "Уютная квартира с видом на парк", Address = "Центральная улица, 1", Area = 50, ImageLinkId = 1 },
            new Property { OwnerId = users[0].Id, Name = "Дом с садом", Desc = "Просторный дом с большим садом", Address = "Садовая улица, 10", Area = 150, ImageLinkId = 2 },
            new Property { OwnerId = users[1].Id, Name = "Студия у моря", Desc = "Студия с выходом на пляж", Address = "Морская улица, 5", Area = 30, ImageLinkId = 3 },
            new Property { OwnerId = users[2].Id, Name = "Коттедж в лесу", Desc = "Коттедж в живописном месте", Address = "Лесная улица, 20", Area = 200, ImageLinkId = 4 },
            new Property { OwnerId = users[2].Id, Name = "Квартира в новостройке", Desc = "Современная квартира с ремонтом", Address = "Новая улица, 15", Area = 75, ImageLinkId = 5 },
            new Property { OwnerId = users[3].Id, Name = "Дача на природе", Desc = "Дача с возможностью рыбалки", Address = "Природная улица, 8", Area = 100, ImageLinkId = 6 },
            new Property { OwnerId = users[4].Id, Name = "Коммерческая недвижимость", Desc = "Помещение для бизнеса", Address = "Бизнес улица, 12", Area = 120, ImageLinkId = 7 },
            new Property { OwnerId = users[5].Id, Name = "Квартира с балконом", Desc = "Квартира с большим балконом", Address = "Балконная улица, 3", Area = 65, ImageLinkId = 8 },
            new Property { OwnerId = users[6].Id, Name = "Уютный дом", Desc = "Дом с камином и уютной атмосферой", Address = "Уютная улица, 4", Area = 90, ImageLinkId = 9 },
            new Property { OwnerId = users[7].Id, Name = "Студия в центре", Desc = "Студия в центре города", Address = "Центр улица, 2", Area = 40, ImageLinkId = 10 },
            new Property { OwnerId = users[8].Id, Name = "Коттедж у озера", Desc = "Коттедж с выходом к озеру", Address = "Озёрная улица, 6", Area = 180, ImageLinkId = 11 },
            new Property { OwnerId = users[9].Id, Name = "Квартира с ремонтом", Desc = "Квартира с новым ремонтом", Address = "Ремонтная улица, 9", Area = 70, ImageLinkId = 12 },
            new Property { OwnerId = users[9].Id, Name = "Дача с участком", Desc = "Дача с большим участком земли", Address = "Участковая улица, 14", Area = 110, ImageLinkId = 13 }
        };

            properties.ForEach(p => context.Properties.Add(p));
            context.SaveChanges();

            _logger.LogInformation("Database was populated with fake data");
        }
    }
}