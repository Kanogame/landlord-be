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


            var addressed = new List<Address> {
            new Address { City = "Москва", District = "Центральный", Street = "Тверская улица", Story = 5 },
            new Address { City = "Санкт-Петербург", District = "Невский", Street = "Невский проспект", Story = 3 },
            new Address { City = "Казань", District = "Вахитовский", Street = "Баумана улица", Story = 7 },
            new Address { City = "Екатеринбург", District = "Центральный", Street = "Ленина улица", Story = 10 },
            new Address { City = "Новосибирск", District = "Центральный", Street = "Красный проспект", Story = 4 },
            new Address { City = "Нижний Новгород", District = "Центральный", Street = "Большая Покровская улица", Story = 6 },
            new Address { City = "Челябинск", District = "Центральный", Street = "Кировский проспект", Story = 8 },
            new Address { City = "Ростов-на-Дону", District = "Центральный", Street = "Садовая улица", Story = 2 },
            new Address { City = "Уфа", District = "Центральный", Street = "Ленина улица", Story = 5 },
            new Address { City = "Волгоград", District = "Центральный", Street = "Мира улица", Story = 3 }
            };

            addressed.ForEach(u => context.Addresses.Add(u));
            context.SaveChanges();



            var properties = new List<Property>
        {
            new Property { OwnerId = users[0].Id, OfferTypeId=OfferType.Rent, PropertyTypeId=PropertyType.Flat, Name = "Дом с садом", Desc = "Просторный дом с большим садом", AddressId = 1, Area = 150, ImageLinkId = 2 },
            new Property { OwnerId = users[1].Id, OfferTypeId=OfferType.Sell, PropertyTypeId=PropertyType.Flat, Name = "Студия у моря", Desc = "Студия с выходом на пляж", AddressId = 2, Area = 30, ImageLinkId = 3 },
            new Property { OwnerId = users[2].Id, OfferTypeId=OfferType.Rent, PropertyTypeId=PropertyType.Detached, Name = "Коттедж в лесу", Desc = "Коттедж в живописном месте", AddressId = 3, Area = 200, ImageLinkId = 4 },
            new Property { OwnerId = users[2].Id, OfferTypeId=OfferType.Rent, PropertyTypeId=PropertyType.Flat, Name = "Квартира в новостройке", Desc = "Современная квартира с ремонтом", AddressId = 4, Area = 75, ImageLinkId = 5 },
            new Property { OwnerId = users[3].Id, OfferTypeId=OfferType.Sell, PropertyTypeId=PropertyType.Flat, Name = "Дача на природе", Desc = "Дача с возможностью рыбалки", AddressId = 5, Area = 100, ImageLinkId = 6 },
            new Property { OwnerId = users[4].Id, OfferTypeId=OfferType.Rent, PropertyTypeId=PropertyType.Detached, Name = "Коммерческая недвижимость", Desc = "Помещение для бизнеса", AddressId = 6, Area = 120, ImageLinkId = 7 },
            new Property { OwnerId = users[5].Id, OfferTypeId=OfferType.Sell, PropertyTypeId=PropertyType.Flat, Name = "Квартира с балконом", Desc = "Квартира с большим балконом", AddressId = 7, Area = 65, ImageLinkId = 8 },
            new Property { OwnerId = users[6].Id, OfferTypeId=OfferType.Rent, PropertyTypeId=PropertyType.Commercial, Name = "Уютный дом", Desc = "Дом с камином и уютной атмосферой", AddressId = 8, Area = 90, ImageLinkId = 9 },
            new Property { OwnerId = users[7].Id, OfferTypeId=OfferType.Sell, PropertyTypeId=PropertyType.Commercial, Name = "Студия в центре", Desc = "Студия в центре города", AddressId = 9, Area = 40, ImageLinkId = 10 },
            new Property { OwnerId = users[8].Id, OfferTypeId=OfferType.Sell, PropertyTypeId=PropertyType.Detached, Name = "Коттедж у озера", Desc = "Коттедж с выходом к озеру", AddressId = 10, Area = 180, ImageLinkId = 11 },
        };

            properties.ForEach(p => context.Properties.Add(p));
            context.SaveChanges();


            _logger.LogInformation("Database was populated with fake data");
        }
    }
}
