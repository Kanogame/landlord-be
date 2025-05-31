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

            // Define attribute pool with 15 unique attribute types
            var attributePool = new List<(
                string Name,
                PropertyAttributeType Type,
                List<string> PossibleValues
            )>
            {
                (
                    "Залог",
                    PropertyAttributeType.Number,
                    new List<string> { "50000", "100000", "150000", "200000", "0" }
                ),
                (
                    "Оплата ЖКХ",
                    PropertyAttributeType.Text,
                    new List<string> { "Включено", "Не включено", "Частично включено" }
                ),
                (
                    "Срок аренды",
                    PropertyAttributeType.Text,
                    new List<string> { "от 1 месяца", "от 3 месяцев", "от 6 месяцев", "от 1 года" }
                ),
                (
                    "Домашние животные",
                    PropertyAttributeType.Text,
                    new List<string> { "Разрешены", "Не разрешены", "По договоренности" }
                ),
                (
                    "Интернет",
                    PropertyAttributeType.Text,
                    new List<string> { "Оптоволокно", "ADSL", "Wi-Fi", "Не подключен" }
                ),
                (
                    "Отопление",
                    PropertyAttributeType.Text,
                    new List<string> { "Центральное", "Автономное", "Электрическое", "Газовое" }
                ),
                (
                    "Этаж",
                    PropertyAttributeType.Number,
                    new List<string> { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" }
                ),
                (
                    "Мебель",
                    PropertyAttributeType.Text,
                    new List<string>
                    {
                        "Полностью меблирована",
                        "Частично меблирована",
                        "Без мебели",
                    }
                ),
                (
                    "Техника",
                    PropertyAttributeType.Text,
                    new List<string> { "Полный набор", "Частично", "Без техники" }
                ),
                (
                    "Ремонт",
                    PropertyAttributeType.Text,
                    new List<string>
                    {
                        "Евроремонт",
                        "Косметический",
                        "Требует ремонта",
                        "Дизайнерский",
                    }
                ),
                (
                    "Парковка",
                    PropertyAttributeType.Text,
                    new List<string> { "Гараж", "Парковочное место", "Во дворе", "Платная", "Нет" }
                ),
                ("Балкон", PropertyAttributeType.Boolean, new List<string> { "true", "false" }),
                ("Лифт", PropertyAttributeType.Boolean, new List<string> { "true", "false" }),
                ("Охрана", PropertyAttributeType.Boolean, new List<string> { "true", "false" }),
                (
                    "Кондиционер",
                    PropertyAttributeType.Boolean,
                    new List<string> { "true", "false" }
                ),
            };

            var propertyAttributes = new List<PropertyAttribute>();
            var random = new Random();

            // Generate attributes for each property
            for (int i = 0; i < 14; i++)
            {
                // Randomly select 10-15 attributes for each property
                var attributeCount = random.Next(10, 16);
                var selectedAttributes = attributePool
                    .OrderBy(x => random.Next())
                    .Take(attributeCount);

                foreach (var (name, type, possibleValues) in selectedAttributes)
                {
                    var value = possibleValues[random.Next(possibleValues.Count)];

                    propertyAttributes.Add(
                        new PropertyAttribute
                        {
                            PropertyId = properties[i].Id,
                            Name = name,
                            Value = value,
                            AttributeType = type,
                            IsSearchable = true,
                        }
                    );
                }
            }

            propertyAttributes.ForEach(a => context.Attributes.Add(a));
            context.SaveChanges();

            var imageLinks = new List<ImageLink>();

            for (int i = 0; i < 14; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    imageLinks.Add(
                        new ImageLink
                        {
                            PropertyId = properties[i].Id,
                            Link =
                                "http://localhost:5249/public/images/"
                                + ((i + 1) % 9)
                                + "_"
                                + (j + 1)
                                + ".webp",
                        }
                    );
                }
            }

            imageLinks.ForEach(i => context.ImageLinks.Add(i));
            context.SaveChanges();

            // Add this after the imageLinks.ForEach(i => context.ImageLinks.Add(i)); line and before context.SaveChanges();

            var calendarEntryPool = new List<(
                string Name,
                string Description,
                CalendarState State,
                int DurationDays
            )>
            {
                (
                    "Ремонт",
                    "Проведен ремонт в квартире, были переклеены обои на кухне и заменена сантехника в ванной комнате.",
                    CalendarState.Maintenance,
                    7
                ),
                (
                    "Генеральная уборка",
                    "Выполнена генеральная уборка после предыдущих жильцов. Помыты все окна, почищены ковры, обработаны все поверхности дезинфицирующими средствами.",
                    CalendarState.Maintenance,
                    2
                ),
                (
                    "Аренда семье Петровых",
                    "Квартира сдана в аренду молодой семье с ребенком. Договор подписан на год с возможностью продления. Залог внесен в полном объеме.",
                    CalendarState.Rented,
                    30
                ),
                (
                    "Косметический ремонт",
                    "Проведена покраска стен в гостиной и спальне, заменены розетки и выключатели. Установлены новые плинтуса по всей квартире.",
                    CalendarState.Maintenance,
                    5
                ),
                (
                    "Долгосрочная аренда",
                    "Квартира сдана студенту на время обучения в университете. Договор заключен на 2 года. Арендатор очень ответственный и аккуратный.",
                    CalendarState.Rented,
                    60
                ),
                (
                    "Замена сантехники",
                    "Полная замена сантехники в ванной комнате и туалете. Установлена новая ванна, унитаз, раковина и смесители. Работы выполнены качественно.",
                    CalendarState.Maintenance,
                    4
                ),
                (
                    "Краткосрочная аренда",
                    "Квартира сдается на месяц командированному специалисту. Все коммунальные услуги включены в стоимость аренды.",
                    CalendarState.Rented,
                    30
                ),
                (
                    "Замена окон",
                    "Установлены новые пластиковые окна во всех комнатах. Значительно улучшилась тепло- и звукоизоляция квартиры.",
                    CalendarState.Maintenance,
                    3
                ),
                (
                    "Подготовка к продаже",
                    "Квартира готовится к продаже. Проведен косметический ремонт, обновлена мебель, сделана профессиональная фотосъемка для объявления.",
                    CalendarState.Blocked,
                    14
                ),
                (
                    "Аренда молодой паре",
                    "Сдано молодой паре без детей и домашних животных. Очень чистоплотные арендаторы, всегда вовремя платят за аренду.",
                    CalendarState.Rented,
                    90
                ),
                (
                    "Ремонт электрики",
                    "Полная замена электропроводки в квартире. Установлены новые автоматы в щитке, добавлены дополнительные розетки в комнатах.",
                    CalendarState.Maintenance,
                    6
                ),
                (
                    "Бронирование",
                    "Квартира забронирована для туристов на новогодние праздники. Предоплата получена, ожидается заселение через неделю.",
                    CalendarState.Reserved,
                    10
                ),
                (
                    "Ремонт пола",
                    "Замена напольного покрытия во всех комнатах. Уложен новый ламинат, установлены пороги. Квартира выглядит как новая.",
                    CalendarState.Maintenance,
                    8
                ),
                (
                    "Семейная аренда",
                    "Квартира сдана семье с двумя детьми школьного возраста. Родители работают в соседнем офисном центре, очень удобное расположение для них.",
                    CalendarState.Rented,
                    180
                ),
                (
                    "Техническое обслуживание",
                    "Проведено плановое техническое обслуживание: проверка всех систем, чистка вентиляции, профилактика бытовой техники.",
                    CalendarState.Maintenance,
                    1
                ),
            };

            var calendarPeriods = new List<CalendarPeriod>();
            var calendarRandom = new Random();

            // Generate calendar entries for first 10 properties
            for (int i = 0; i < 10; i++)
            {
                var currentDate = DateTime.UtcNow.AddDays(-90); // Start from 3 months ago
                var propertyId = properties[i].Id;
                var ownerId = properties[i].OwnerId;

                // Generate 2-4 calendar entries per property
                var entryCount = calendarRandom.Next(2, 5);

                for (int j = 0; j < entryCount; j++)
                {
                    var entry = calendarEntryPool[calendarRandom.Next(calendarEntryPool.Count)];
                    var startDate = currentDate.AddDays(calendarRandom.Next(0, 30));
                    var endDate = startDate.AddDays(
                        entry.DurationDays + calendarRandom.Next(-2, 5)
                    );

                    // For rented periods, sometimes attach a user
                    int? attachedUserId = null;
                    if (entry.State == CalendarState.Rented && calendarRandom.Next(0, 2) == 0)
                    {
                        // Attach a random user that's not the owner
                        var availableUsers = users.Where(u => u.Id != ownerId).ToList();
                        if (availableUsers.Any())
                        {
                            attachedUserId = availableUsers[
                                calendarRandom.Next(availableUsers.Count)
                            ].Id;
                        }
                    }

                    calendarPeriods.Add(
                        new CalendarPeriod
                        {
                            PropertyId = propertyId,
                            StartDate = startDate,
                            EndDate = endDate,
                            State = entry.State,
                            Name = entry.Name,
                            Description = entry.Description,
                            AttachedUserId = attachedUserId,
                            CreatedAt = DateTime.UtcNow.AddDays(-calendarRandom.Next(1, 30)),
                            UpdatedAt = DateTime.UtcNow.AddDays(-calendarRandom.Next(0, 10)),
                        }
                    );

                    // Move current date forward to avoid overlaps
                    currentDate = endDate.AddDays(calendarRandom.Next(1, 15));
                }
            }

            calendarPeriods.ForEach(cp => context.CalendarPeriods.Add(cp));
            context.SaveChanges();
        }
    }
}
