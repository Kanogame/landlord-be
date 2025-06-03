using landlord_be.Models;
using Microsoft.EntityFrameworkCore;

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

        public async Task Initialize(ApplicationDbContext context)
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
                Patronym = "Иванович"
            },
            new Personal
            {
                FirstName = "Александр",
                LastName = "Петров",
                Patronym = "Сергеевич"
            },
            new Personal
            {
                FirstName = "Мария",
                LastName = "Соколова",
                Patronym = "Алексеевна"
            },
            new Personal
            {
                FirstName = "Екатерина",
                LastName = "Николаева",
                Patronym = "Викторовна"
            },
            new Personal
            {
                FirstName = "Дмитрий",
                LastName = "Кузнецов",
                Patronym = "Петрович"
            },
            new Personal
            {
                FirstName = "Ольга",
                LastName = "Морозова",
                Patronym = "Андреевна"
            },
            new Personal
            {
                FirstName = "Сергей",
                LastName = "Сидоров",
                Patronym = "Николаевич"
            },
            new Personal
            {
                FirstName = "Наталья",
                LastName = "Лебедева",
                Patronym = "Ивановна"
            },
            new Personal
            {
                FirstName = "Андрей",
                LastName = "Смирнов",
                Patronym = "Юрьевич"
            },
            new Personal
            {
                FirstName = "Елена",
                LastName = "Фёдорова",
                Patronym = "Григорьевна"
            },
            new Personal
            {
                FirstName = "Владимир",
                LastName = "Михайлов",
                Patronym = "Васильевич"
            },
            new Personal
            {
                FirstName = "Татьяна",
                LastName = "Григорьева",
                Patronym = "Анатольевна"
            },
            new Personal
            {
                FirstName = "Максим",
                LastName = "Савельев",
                Patronym = "Дмитриевич"
            },
            new Personal
            {
                FirstName = "Юлия",
                LastName = "Борисова",
                Patronym = "Олеговна"
            },
            new Personal
            {
                FirstName = "Роман",
                LastName = "Юрьев",
                Patronym = "Михайлович"
            }
        };


            personals.ForEach(p => context.Personals.Add(p));
            await context.SaveChangesAsync();

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
            await context.SaveChangesAsync();

            var addresses = new List<Address>
        {
            new Address
            {
                City = "Екатеринбург",
                Region = "Свердловская обл",
                Street = "Ленина",
                House = "35",
                Floor = 2,
            },
            new Address
            {
                City = "Москва",
                Region = "Москва",
                Street = "Тверская",
                House = "15",
                Floor = 5,
            },
            new Address
            {
                City = "Санкт-Петербург",
                Region = "Ленинградская обл",
                Street = "Невский проспект",
                House = "28",
                Floor = 3,
            },
            new Address
            {
                City = "Новосибирск",
                Region = "Новосибирская обл",
                Street = "Красный проспект",
                House = "80",
                Floor = 4,
            },
            new Address
            {
                City = "Казань",
                Region = "Республика Татарстан",
                Street = "Пушкина",
                House = "12",
                Floor = 1,
            },
            new Address
            {
                City = "Нижний Новгород",
                Region = "Нижегородская обл",
                Street = "Большая Покровская",
                House = "3A",
                Floor = 6,
            },
            new Address
            {
                City = "Челябинск",
                Region = "Челябинская обл",
                Street = "Мира",
                House = "22Б",
                Floor = 2,
            },
            new Address
            {
                City = "Пермь",
                Region = "Пермский край",
                Street = "Кирова",
                House = "45",
                Floor = 3,
            },
            new Address
            {
                City = "Ростов-на-Дону",
                Region = "Ростовская обл",
                Street = "Гагарина",
                House = "10",
                Floor = 1,
            },
            new Address
            {
                City = "Уфа",
                Region = "Республика Башкортостан",
                Street = "Сергеева",
                House = "7",
                Floor = 4,
            },
            new Address
            {
                City = "Воронеж",
                Region = "Воронежская обл",
                Street = "Ленина",
                House = "18",
                Floor = 2,
            },
            new Address
            {
                City = "Красноярск",
                Region = "Красноярский край",
                Street = "Советская",
                House = "55",
                Floor = 5,
            },
            new Address
            {
                City = "Самара",
                Region = "Самарская обл",
                Street = "Мира",
                House = "30",
                Floor = 3,
            },
            new Address
            {
                City = "Омск",
                Region = "Омская обл",
                Street = "Ленинская",
                House = "42",
                Floor = 2,
            },
            new Address
            {
                City = "Владивосток",
                Region = "Приморский край",
                Street = "Первомайская",
                House = "8",
                Floor = 1,
            }
        };


        addresses.ForEach(a => context.Addresses.Add(a));
        await context.SaveChangesAsync();

            var properties = new List<Property>
            {
                new Property
                {
                    OwnerId = users[0].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Уютная квартира",
                    Desc = "Сдам в аренду стильную квартиру с современным ремонтом в центре города. Светлые комнаты, новые коммуникации и современная встроенная техника создают атмосферу комфорта. Уютный балкон с видом на зеленый сквер дополняет общее впечатление. Район насыщен культурными местами, ресторанами и парками. Идеально для тех, кто ценит городской ритм и удобства.",
                    AddressId = addresses[0].Id,
                    Area = 75,
                    Rooms = 2,
                    Services = true,
                    Parking = false,
                    Price = 50000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.5f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[1].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Семейный дом",
                    Desc = "Продам дом в тихом уютном районе за городом, окружённый зелёным лесом и чистым воздухом. Дом оснащён современной системой отопления, просторной гостиной и уютной кухней. Большой сад с плодовыми деревьями и возможностью постройки летней кухни – идеальное место для семейного отдыха и вечеринок на свежем воздухе.",
                    AddressId = addresses[1].Id,
                    Area = 200,
                    Rooms = 5,
                    Services = true,
                    Parking = true,
                    Price = 12000000,
                    Currency = 125,
                    Rating = 4.8f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[2].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Студия",
                    Desc = "Сдаю современную квартиру с дизайнерским ремонтом в деловом районе. Большие окна дарят много света, а панорамный вид на город создает неповторимую атмосферу. В шаговой доступности крупные торговые центры, офисные здания и деловые клубы. Отличный вариант для молодых профессионалов.",
                    AddressId = addresses[2].Id,
                    Area = 40,
                    Rooms = 1,
                    Services = false,
                    Parking = false,
                    Price = 30000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.0f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[3].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира с балконом",
                    Desc = "Предлагается уютная квартира с обновленным интерьером в центре большого города. Просторная кухня, стильная гостиная и удобные спальни помогут создать атмосферу домашнего уюта. Рядом с домом расположены парки и транспортная развязка, что позволяет легко добраться до любой точки города",
                    AddressId = addresses[3].Id,
                    Area = 85,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 8000000,
                    Currency = 125,
                    Rating = 4.6f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[4].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Дача",
                    Desc = "Предлагаю в аренду дачу с живописным видом на озеро, где можно насладиться спокойствием и природной красотой. Дом выполнен в классическом стиле с просторной террасой, оборудован современной сантехникой и отоплением на дровах. Идеальное место для летних каникул, творческих встреч и семейных праздников.",
                    AddressId = addresses[4].Id,
                    Area = 100,
                    Rooms = 4,
                    Services = true,
                    Parking = true,
                    Price = 20000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.2f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[5].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира в новостройке",
                    Desc = "Продам современную квартиру с открытой планировкой и качественным ремонтом в новом жилом комплексе. Высокие потолки, балкон и просторные комнаты порадуют даже самых взыскательных арендаторов. Дом оборудован системами безопасности и рядом с ним находятся все необходимые объекты инфраструктуры",
                    AddressId = addresses[5].Id,
                    Area = 120,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 15000000,
                    Currency = 125,
                    Rating = 5.0f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[6].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира-студия",
                    Desc = "Аренда уютной квартиры в центре с недавним ремонтом и всеми современными удобствами. Отличная планировка, светлые комнаты, и отдельная кухонная зона создают пространство для творчества. Рядом множество кафе, магазинов и зеленых аллей, где можно насладиться утренним кофе.",
                    AddressId = addresses[6].Id,
                    Area = 50,
                    Rooms = 1,
                    Services = false,
                    Parking = false,
                    Price = 35000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.3f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[7].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Коттедж",
                    Desc = "Продам в аренду современный коттедж в элитном загородном поселке. Дом имеет уникальный архитектурный стиль, большое количество стеклянных элементов, что обеспечивает обилие естественного освещения. Просторный участок с ландшафтным дизайном, беседка и бассейн – всё для комфортного проживания и отдыха.",
                    AddressId = addresses[7].Id,
                    Area = 250,
                    Rooms = 6,
                    Services = true,
                    Parking = true,
                    Price = 20000000,
                    Currency = 125,
                    Rating = 4.9f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[8].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Комфортная квартира",
                    Desc = "Квартира с хорошей планировкой и ремонтом.",
                    AddressId = addresses[8].Id,
                    Area = 70,
                    Rooms = 2,
                    Services = true,
                    Parking = true,
                    Price = 45000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.4f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[9].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира с террасой",
                    Desc = "Сдается в аренду красивая квартира на среднем этаже с ремонтом премиум-класса. Интерьер выполнен в современном стиле, используются качественные материалы и техника. Расположение квартиры в историческом районе с насыщенной культурной жизнью подчеркнет ваш утонченный вкус",
                    AddressId = addresses[9].Id,
                    Area = 95,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 9500000,
                    Currency = 125,
                    Rating = 4.7f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[0].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Дача с видом на озеро",
                    Desc = "Предлагаю в аренду дачу с живописным видом на озеро, где можно насладиться спокойствием и природной красотой. Дом выполнен в классическом стиле с просторной террасой, оборудован современной сантехникой и отоплением на дровах. Идеальное место для летних каникул, творческих встреч и семейных праздников.",
                    AddressId = addresses[10].Id,
                    Area = 180,
                    Rooms = 4,
                    Services = true,
                    Parking = true,
                    Price = 60000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.6f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[1].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира в новостройке",
                    Desc = "Сдам в аренду уютное жилье в центре со свежим ремонтом и функциональной планировкой. Просторная гостиная с большими окнами, современная кухня и комфортабельные спальни – всё для вашего удобства. Район развитой инфраструктуры с магазинами, парками и транспортными узлами",
                    AddressId = addresses[11].Id,
                    Area = 80,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 8500000,
                    Currency = 125,
                    Rating = 4.8f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[2].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Flat,
                    Name = "Квартира с видом на город",
                    Desc = "Предлагаю в аренду светлую и просторную квартиру с обновленным интерьером в престижном районе. Высокое качество отделки, встроенные шкафы и современные системы отопления подарят комфорт в любое время года. Вокруг – развитая сеть кафе, магазинов и зон отдыха.",
                    AddressId = addresses[12].Id,
                    Area = 90,
                    Rooms = 3,
                    Services = true,
                    Parking = true,
                    Price = 70000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.5f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[3].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Detached,
                    Name = "Деревянный дом",
                    Desc = "Продаю деревянный дом с ремонтами под ключ в живописном месте. Дом представляет собой сочетание классического и модернистского стилей: светлые комнаты, современная кухня и санузлы с новой сантехникой. Идеальное место для постоянного проживания или сезонного отдыха в окружении природы",
                    AddressId = addresses[13].Id,
                    Area = 150,
                    Rooms = 5,
                    Services = true,
                    Parking = true,
                    Price = 11000000,
                    Currency = 125,
                    Rating = 4.9f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[0].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Офис в центре города",
                    Desc = "Современный офис с открытой планировкой и всеми удобствами.",
                    AddressId = addresses[0].Id,
                    Area = 120,
                    Rooms = 1,
                    Services = true,
                    Parking = true,
                    Price = 80000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.5f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[1].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Торговое помещение",
                    Desc = "Помещение для магазина в проходимом месте.",
                    AddressId = addresses[1].Id,
                    Area = 150,
                    Rooms = 1,
                    Services = true,
                    Parking = true,
                    Price = 15000000,
                    Currency = 125,
                    Rating = 4.7f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[2].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Коворкинг",
                    Desc = "Просторный коворкинг с удобными рабочими местами.",
                    AddressId = addresses[2].Id,
                    Area = 200,
                    Rooms = 1,
                    Services = true,
                    Parking = true,
                    Price = 60000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.6f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[3].Id,
                    OfferTypeId = OfferType.Sell,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Складское помещение",
                    Desc = "Склад с удобным доступом и охраной.",
                    AddressId = addresses[3].Id,
                    Area = 300,
                    Rooms = 0,
                    Services = true,
                    Parking = true,
                    Price = 5000000,
                    Currency = 125,
                    Rating = 4.4f,
                    Status = PropertyStatus.Active,
                },
                new Property
                {
                    OwnerId = users[4].Id,
                    OfferTypeId = OfferType.Rent,
                    PropertyTypeId = PropertyType.Commercial,
                    Name = "Ресторан",
                    Desc = "Ресторан с полностью оборудованной кухней и залом.",
                    AddressId = addresses[4].Id,
                    Area = 250,
                    Rooms = 2,
                    Services = true,
                    Parking = true,
                    Price = 120000,
                    Currency = 125,
                    Period = RentPeriod.Month,
                    Rating = 4.8f,
                    Status = PropertyStatus.Active,
                },
            };

            properties.ForEach(p => context.Properties.Add(p));
            await context.SaveChangesAsync();

            var savedProperties = await context.Properties.ToListAsync();

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
            for (int i = 0; i < savedProperties.Count; i++)
            {
                // Randomly select 5-9 attributes for each property
                var attributeCount = random.Next(5, 10);
                var selectedAttributes = attributePool
                    .OrderBy(x => random.Next())
                    .Take(attributeCount);

                foreach (var (name, type, possibleValues) in selectedAttributes)
                {
                    var value = possibleValues[random.Next(possibleValues.Count)];

                    propertyAttributes.Add(
                        new PropertyAttribute
                        {
                            PropertyId = savedProperties[i].Id,
                            Name = name,
                            Value = value,
                            AttributeType = type,
                            IsSearchable = true,
                        }
                    );
                }
            }

            propertyAttributes.ForEach(a => context.Attributes.Add(a));
            await context.SaveChangesAsync();

            var imageList = ImageLink[][]{
                [
                    "public/images/flats/1_1.webp",
                    "public/images/flats/1_2.webp",
                    "public/images/flats/1_3.webp",
                    "public/images/flats/1_4.webp",
                    "public/images/flats/1_5.webp",
                    "public/images/flats/1_6.webp",
                    "public/images/flats/1_7.webp",
                    "public/images/flats/1_8.webp"
                ],
                [
                    "public/images/houses/1_1.webp",
                    "public/images/houses/1_2.webp",
                    "public/images/houses/1_3.webp",
                    "public/images/houses/1_4.webp",
                ],
                [
                    "public/images/flats/2_1.webp",
                    "public/images/flats/2_2.webp",
                    "public/images/flats/2_3.webp",
                    "public/images/flats/2_4.webp",
                    "public/images/flats/2_5.webp",
                ],
                [
                    "public/images/flats/3_1.webp",
                    "public/images/flats/3_2.webp",
                    "public/images/flats/3_3.webp",
                    "public/images/flats/3_4.webp",
                    "public/images/flats/3_5.webp",
                ],
                 [
                    "public/images/houses/2_1.webp",
                    "public/images/houses/2_2.webp",
                    "public/images/houses/2_3.webp",
                    "public/images/houses/2_4.webp",
                    "public/images/houses/2_5.webp",
                ],

                [
                    "public/images/flats/4_1.webp",
                    "public/images/flats/4_2.webp",
                    "public/images/flats/4_3.webp",
                    "public/images/flats/4_4.webp",
                    "public/images/flats/4_5.webp",
                ],
                [
                    "public/images/flats/5_1.webp",
                    "public/images/flats/5_2.webp",
                    "public/images/flats/5_3.webp",
                    "public/images/flats/5_4.webp",
                    "public/images/flats/5_5.webp",
                ],
                  [
                    "public/images/houses/3_1.webp",
                    "public/images/houses/3_2.webp",
                    "public/images/houses/3_3.webp",
                    "public/images/houses/3_4.webp",
                    "public/images/houses/3_5.webp",
                ],

                [
                    "public/images/flats/6_1.webp",
                    "public/images/flats/6_2.webp",
                    "public/images/flats/6_3.webp",
                    "public/images/flats/6_4.webp",
                    "public/images/flats/6_5.webp",
                ],
                 [
                    "public/images/flats/7_1.webp",
                    "public/images/flats/7_2.webp",
                    "public/images/flats/7_3.webp",
                    "public/images/flats/7_4.webp",
                    "public/images/flats/7_5.webp",
                ],
                [
                    "public/images/houses/4_1.webp",
                    "public/images/houses/4_2.webp",
                    "public/images/houses/4_3.webp",
                    "public/images/houses/4_4.webp",
                    "public/images/houses/4_5.webp",
                ],
                 [
                    "public/images/flats/8_1.webp",
                    "public/images/flats/8_2.webp",
                    "public/images/flats/8_3.webp",
                    "public/images/flats/8_4.webp",
                    "public/images/flats/8_5.webp",
                ],
                  [
                    "public/images/flats/9_1.webp",
                    "public/images/flats/9_2.webp",
                    "public/images/flats/9_3.webp",
                    "public/images/flats/9_4.webp",
                    "public/images/flats/9_5.webp",
                ],
                 [
                    "public/images/houses/5_1.webp",
                    "public/images/houses/5_2.webp",
                    "public/images/houses/5_3.webp",
                    "public/images/houses/5_4.webp",
                    "public/images/houses/5_5.webp",
                ],
            }

            var imageLinks = new List<ImageLink>();

            for (int i = 0; i < savedProperties.Count; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    imageLinks.Add(
                        new ImageLink
                        {
                            PropertyId = savedProperties[i].Id,
                            Link = "public/images/" + ((i + 1) % 8) + "_" + (j + 1) + ".webp",
                        }
                    );
                }
            }

            imageLinks.ForEach(i => context.ImageLinks.Add(i));
            await context.SaveChangesAsync();

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
            for (int i = 0; i < savedProperties.Count; i++)
            {
                var currentDate = DateTime.UtcNow.AddDays(-90); // Start from 3 months ago
                var propertyId = savedProperties[i].Id;
                var ownerId = savedProperties[i].OwnerId;

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
            await context.SaveChangesAsync();
        }
    }
}
