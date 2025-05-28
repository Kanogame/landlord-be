using landlord_be.Models;

#pragma warning disable IDE0090
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
            var rd = new Random();

            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            DateTime time = DateTime.Now.ToUniversalTime();

            var personals = new List<Personal>
            {
                new Personal
                {
                    FirstName = "Иван",
                    LastName = "Иванов",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Петр",
                    LastName = "Петров",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Сергей",
                    LastName = "Сергеев",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Алексей",
                    LastName = "Алексеев",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Дмитрий",
                    LastName = "Дмитриев",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Анна",
                    LastName = "Антонова",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Мария",
                    LastName = "Маркова",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Елена",
                    LastName = "Еленина",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Олег",
                    LastName = "Олегов",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Т1атьяна",
                    LastName = "Татьянова",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Т2атьяна",
                    LastName = "Татьянова",
                    Patronym = "Иванович",
                },
                new Personal
                {
                    FirstName = "Т3атьяна",
                    LastName = "Татьянова",
                    Patronym = "Иванович",
                },
            };

            personals.ForEach(p => context.Personals.Add(p));
            context.SaveChanges();

            var users = new List<User>();

            for (int i = 0; i < 11; i++)
            {
                users.Add(
                    new User
                    {
                        PersonalId = personals[i].Id,
                        NumberHash = "hash1",
                        Email = "some@mail.ru",
                        Token = "token1",
                        RegisterDate = time,
                        UpdateDate = time,
                    }
                );
            }

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            var addresses = new List<Address>
            {
                new Address
                {
                    City = "Москва",
                    District = "Центральный",
                    Street = "Тверская улица",
                    Story = 5,
                },
                new Address
                {
                    City = "Санкт-Петербург",
                    District = "Невский",
                    Street = "Невский проспект",
                    Story = 3,
                },
                new Address
                {
                    City = "Казань",
                    District = "Вахитовский",
                    Street = "Баумана улица",
                    Story = 7,
                },
                new Address
                {
                    City = "Екатеринбург",
                    District = "Центральный",
                    Street = "Ленина улица",
                    Story = 10,
                },
                new Address
                {
                    City = "Новосибирск",
                    District = "Центральный",
                    Street = "Красный проспект",
                    Story = 4,
                },
                new Address
                {
                    City = "Нижний Новгород",
                    District = "Центральный",
                    Street = "Большая Покровская улица",
                    Story = 6,
                },
                new Address
                {
                    City = "Челябинск",
                    District = "Центральный",
                    Street = "Кировский проспект",
                    Story = 8,
                },
                new Address
                {
                    City = "Ростов-на-Дону",
                    District = "Центральный",
                    Street = "Садовая улица",
                    Story = 2,
                },
                new Address
                {
                    City = "Уфа",
                    District = "Центральный",
                    Street = "Ленина улица",
                    Story = 5,
                },
                new Address
                {
                    City = "Волгоград",
                    District = "Центральный",
                    Street = "Мира улица",
                    Story = 3,
                },
                new Address
                {
                    City = "Краснодар",
                    District = "Центральный",
                    Street = "Красная улица",
                    Story = 12,
                },
                new Address
                {
                    City = "Воронеж",
                    District = "Центральный",
                    Street = "Проспект Революции",
                    Story = 9,
                },
                new Address
                {
                    City = "Пермь",
                    District = "Центральный",
                    Street = "Комсомольский проспект",
                    Story = 6,
                },
                new Address
                {
                    City = "Самара",
                    District = "Центральный",
                    Street = "Молодогвардейская улица",
                    Story = 14,
                },
                new Address
                {
                    City = "Омск",
                    District = "Центральный",
                    Street = "Ленина улица",
                    Story = 7,
                },
            };

            addresses.ForEach(a => context.Addresses.Add(a));
            context.SaveChanges();

            var properties = new List<Property>
            {
                new Property
                {
                    OwnerId = users[0].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Просторная 3-комнатная квартира",
                    Desc =
                        " Lorem ipsum dolor sit amet, consectetur adipiscing elit. Maecenas mattis aliquet lacus nec bibendum. Sed rutrum nunc eros, accumsan sagittis  libero gravida id. Duis purus libero, posuere quis rutrum in, bibendum  eget ex. Morbi magna nunc, accumsan in fermentum vel, sollicitudin quis  arcu. Pellentesque urna libero, euismod scelerisque sollicitudin nec,  venenatis eget lectus. Integer molestie nulla in luctus iaculis. Donec  finibus semper urna in tempus. Nam nec dui quis lectus fringilla  ullamcorper. Integer at porttitor arcu, ac dignissim lectus. Quisque maximus tempor ante, vel cursus orci euismod eu. Morbi ut nisi  ornare, tincidunt justo non, lobortis augue. Donec nisl nibh, iaculis  sit amet ultrices sit amet, tincidunt a est. Pellentesque non leo nec  ipsum auctor eleifend. ",
                    AddressId = addresses[0].Id,
                    Area = 85,
                    Price = 75000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Raiting = 4.5f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[1].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Студия у моря",
                    Desc = "Студия с выходом на пляж",
                    AddressId = addresses[1].Id,
                    Area = 30,
                    Price = 3500000,
                    Currency = 125,
                    Raiting = 4.2f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[2].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Коттедж в лесу",
                    Desc = "Коттедж в живописном месте",
                    AddressId = addresses[2].Id,
                    Area = 200,
                    Price = 150000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Raiting = 4.8f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[2].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира в новостройке",
                    Desc = "Современная квартира с ремонтом",
                    AddressId = addresses[3].Id,
                    Area = 75,
                    Price = 55000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Raiting = 4.3f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[3].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Дача на природе",
                    Desc = "Дача с возможностью рыбалки",
                    AddressId = addresses[4].Id,
                    Area = 100,
                    Price = 2800000,
                    Currency = 125,
                    Raiting = 4.0f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[4].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Офисное помещение",
                    Desc = "Помещение для бизнеса в центре",
                    AddressId = addresses[5].Id,
                    Area = 120,
                    Price = 180000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Raiting = 4.1f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[5].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира с балконом",
                    Desc = "Квартира с большим балконом",
                    AddressId = addresses[6].Id,
                    Area = 65,
                    Price = 4200000,
                    Currency = 125,
                    Raiting = 4.4f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[6].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Уютный дом",
                    Desc = "Дом с камином и уютной атмосферой",
                    AddressId = addresses[7].Id,
                    Area = 90,
                    Price = 85000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Raiting = 4.6f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[7].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Студия в центре",
                    Desc = "Студия в центре города",
                    AddressId = addresses[8].Id,
                    Area = 40,
                    Price = 2900000,
                    Currency = 125,
                    Raiting = 3.9f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[8].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Коттедж у озера",
                    Desc = "Коттедж с выходом к озеру",
                    AddressId = addresses[9].Id,
                    Area = 180,
                    Price = 8500000,
                    Currency = 125,
                    Raiting = 4.9f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[9].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Торговое помещение",
                    Desc = "Помещение на первом этаже",
                    AddressId = addresses[10].Id,
                    Area = 95,
                    Price = 120000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Raiting = 4.2f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[0].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Однокомнатная квартира",
                    Desc = "Уютная квартира для молодой семьи",
                    AddressId = addresses[11].Id,
                    Area = 45,
                    Price = 35000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Raiting = 4.1f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[1].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Складское помещение",
                    Desc = "Помещение для хранения товаров",
                    AddressId = addresses[12].Id,
                    Area = 300,
                    Price = 15000000,
                    Currency = 125,
                    Raiting = 3.8f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[3].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Пентхаус",
                    Desc = "Роскошный пентхаус с панорамным видом",
                    AddressId = addresses[13].Id,
                    Area = 150,
                    Price = 250000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Raiting = 5.0f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[4].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Загородный дом",
                    Desc = "Дом для отдыха на выходные",
                    AddressId = addresses[14].Id,
                    Area = 110,
                    Price = 15000,
                    Currency = 125,
                    Period = RentPeriod.Day,
                    Raiting = 4.3f,
                    Status = PropertyStatus.Active,
                },
            };

            properties.ForEach(p => context.Properties.Add(p));
            context.SaveChanges();

            var imageLinks = new List<ImageLink>();

            for (int i = 0; i < 12; i++)
            {
                int id = rd.Next(0, properties.Count);
                imageLinks.Add(
                    new ImageLink
                    {
                        PropertyId = properties[id].Id,
                        Link = "https://example.com/images/1" + (id + 1) + "_1.jpg",
                    }
                );
            }

            imageLinks.ForEach(i => context.ImageLinks.Add(i));
            context.SaveChanges();
        }
    }
}
