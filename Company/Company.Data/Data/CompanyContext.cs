using Company.Data.Data.CMS;
using Company.Data.Data.Shop;
using Microsoft.EntityFrameworkCore;

namespace Company.Data.Data;

public class CompanyContext : DbContext
{
    public CompanyContext (DbContextOptions<CompanyContext> options)
        : base(options)
    {
    }

    public DbSet<Page> Page { get; set; } = default!;
    public DbSet<News> News { get; set; } = default!;
    public DbSet<TypeOfProduct> TypeOfProduct { get; set; } = default!;
    public DbSet<Product> Product { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed Pages (CMS)
        modelBuilder.Entity<Page>().HasData(
            new Page
            {
                IdPage = 1,
                LinkTitle = "O nas",
                Title = "O naszej firmie",
                Content = "<p>Jesteśmy firmą z wieloletnim doświadczeniem na rynku. Specjalizujemy się w dostarczaniu najwyższej jakości produktów i usług dla naszych klientów.</p><p>Nasza misja to zapewnienie satysfakcji każdego klienta poprzez profesjonalną obsługę i konkurencyjne ceny.</p>",
                DisplayOrder = 1
            },
            new Page
            {
                IdPage = 2,
                LinkTitle = "Kontakt",
                Title = "Skontaktuj się z nami",
                Content = "<p><strong>Adres:</strong> ul. Przykładowa 123, 00-001 Warszawa</p><p><strong>Telefon:</strong> +48 123 456 789</p><p><strong>Email:</strong> kontakt@firma.pl</p><p>Zapraszamy od poniedziałku do piątku w godzinach 8:00 - 16:00</p>",
                DisplayOrder = 2
            },
            new Page
            {
                IdPage = 3,
                LinkTitle = "Regulamin",
                Title = "Regulamin sklepu",
                Content = "<h3>§1 Postanowienia ogólne</h3><p>Niniejszy regulamin określa zasady korzystania ze sklepu internetowego.</p><h3>§2 Składanie zamówień</h3><p>Zamówienia można składać 24 godziny na dobę, 7 dni w tygodniu.</p><h3>§3 Płatności</h3><p>Akceptujemy płatności kartą, przelewem oraz za pobraniem.</p>",
                DisplayOrder = 3
            },
            new Page
            {
                IdPage = 4,
                LinkTitle = "Polityka prywatności",
                Title = "Polityka prywatności",
                Content = "<p>Szanujemy Twoją prywatność. Wszystkie dane osobowe są przetwarzane zgodnie z RODO.</p><p>Twoje dane wykorzystujemy wyłącznie w celu realizacji zamówień i nie udostępniamy ich podmiotom trzecim.</p>",
                DisplayOrder = 4
            }
        );

        // Seed News (CMS)
        modelBuilder.Entity<News>().HasData(
            new News
            {
                IdNews = 1,
                LinkTitle = "Wielka wyprzedaż",
                Title = "Wielka wyprzedaż zimowa!",
                Content = "<p>Zapraszamy na wielką wyprzedaż zimową! Rabaty do 50% na wybrane produkty. Promocja trwa do końca miesiąca.</p><p>Nie przegap okazji!</p>",
                DisplayOrder = 1
            },
            new News
            {
                IdNews = 2,
                LinkTitle = "Nowa kolekcja",
                Title = "Nowa kolekcja już dostępna",
                Content = "<p>Z przyjemnością informujemy, że nowa kolekcja produktów jest już dostępna w naszym sklepie.</p><p>Sprawdź najnowsze trendy i wybierz coś dla siebie!</p>",
                DisplayOrder = 2
            },
            new News
            {
                IdNews = 3,
                LinkTitle = "Darmowa dostawa",
                Title = "Darmowa dostawa od 100 zł",
                Content = "<p>Miło nam poinformować, że od teraz oferujemy darmową dostawę przy zamówieniach powyżej 100 zł.</p><p>Zamów już dziś i skorzystaj z promocji!</p>",
                DisplayOrder = 3
            },
            new News
            {
                IdNews = 4,
                LinkTitle = "Godziny świąteczne",
                Title = "Zmiana godzin pracy",
                Content = "<p>W okresie świątecznym pracujemy w zmienionych godzinach:</p><ul><li>24.12 - 8:00 - 12:00</li><li>25-26.12 - zamknięte</li><li>31.12 - 8:00 - 14:00</li></ul>",
                DisplayOrder = 4
            }
        );

        // Seed TypeOfProduct (Shop)
        modelBuilder.Entity<TypeOfProduct>().HasData(
            new TypeOfProduct
            {
                IdTypeOfProduct = 1,
                Name = "Elektronika",
                Description = "Smartfony, tablety, laptopy i inne urządzenia elektroniczne"
            },
            new TypeOfProduct
            {
                IdTypeOfProduct = 2,
                Name = "Odzież",
                Description = "Koszulki, spodnie, kurtki i inne ubrania"
            },
            new TypeOfProduct
            {
                IdTypeOfProduct = 3,
                Name = "Dom i ogród",
                Description = "Meble, dekoracje, narzędzia ogrodowe"
            },
            new TypeOfProduct
            {
                IdTypeOfProduct = 4,
                Name = "Sport",
                Description = "Sprzęt sportowy, odzież sportowa, akcesoria fitness"
            },
            new TypeOfProduct
            {
                IdTypeOfProduct = 5,
                Name = "Książki",
                Description = "Literatura, podręczniki, audiobooki"
            }
        );

        // Seed Product (Shop)
        modelBuilder.Entity<Product>().HasData(
            // Elektronika
            new Product
            {
                IdProduct = 1,
                Code = "ELEC-001",
                Name = "Smartfon Galaxy X",
                Price = 2499.99m,
                FotoURL = "https://via.placeholder.com/300x300?text=Smartfon",
                Description = "Nowoczesny smartfon z ekranem AMOLED 6.5\", 128GB pamięci, aparat 48MP",
                IsDiscount = true,
                IdTypeOfProduct = 1
            },
            new Product
            {
                IdProduct = 2,
                Code = "ELEC-002",
                Name = "Laptop ProBook 15",
                Price = 3999.00m,
                FotoURL = "https://via.placeholder.com/300x300?text=Laptop",
                Description = "Wydajny laptop z procesorem i7, 16GB RAM, dysk SSD 512GB",
                IsDiscount = false,
                IdTypeOfProduct = 1
            },
            new Product
            {
                IdProduct = 3,
                Code = "ELEC-003",
                Name = "Słuchawki Bluetooth",
                Price = 299.99m,
                FotoURL = "https://via.placeholder.com/300x300?text=Sluchawki",
                Description = "Bezprzewodowe słuchawki z aktywną redukcją szumów, 30h pracy",
                IsDiscount = true,
                IdTypeOfProduct = 1
            },
            // Odzież
            new Product
            {
                IdProduct = 4,
                Code = "CLOTH-001",
                Name = "Koszulka Basic",
                Price = 49.99m,
                FotoURL = "https://via.placeholder.com/300x300?text=Koszulka",
                Description = "Wygodna koszulka z bawełny organicznej, dostępna w wielu kolorach",
                IsDiscount = false,
                IdTypeOfProduct = 2
            },
            new Product
            {
                IdProduct = 5,
                Code = "CLOTH-002",
                Name = "Jeansy Slim Fit",
                Price = 149.99m,
                FotoURL = "https://via.placeholder.com/300x300?text=Jeansy",
                Description = "Klasyczne jeansy o dopasowanym kroju, wysoka jakość denimu",
                IsDiscount = true,
                IdTypeOfProduct = 2
            },
            new Product
            {
                IdProduct = 6,
                Code = "CLOTH-003",
                Name = "Kurtka zimowa",
                Price = 399.00m,
                FotoURL = "https://via.placeholder.com/300x300?text=Kurtka",
                Description = "Ciepła kurtka zimowa z kapturem, wodoodporna, rozmiary S-XXL",
                IsDiscount = false,
                IdTypeOfProduct = 2
            },
            // Dom i ogród
            new Product
            {
                IdProduct = 7,
                Code = "HOME-001",
                Name = "Lampa stołowa LED",
                Price = 129.00m,
                FotoURL = "https://via.placeholder.com/300x300?text=Lampa",
                Description = "Elegancka lampa stołowa LED z regulacją jasności",
                IsDiscount = false,
                IdTypeOfProduct = 3
            },
            new Product
            {
                IdProduct = 8,
                Code = "HOME-002",
                Name = "Doniczka ceramiczna",
                Price = 39.99m,
                FotoURL = "https://via.placeholder.com/300x300?text=Doniczka",
                Description = "Stylowa doniczka ceramiczna, idealna na kwiaty doniczkowe",
                IsDiscount = true,
                IdTypeOfProduct = 3
            },
            // Sport
            new Product
            {
                IdProduct = 9,
                Code = "SPORT-001",
                Name = "Hantle 2x5kg",
                Price = 89.99m,
                FotoURL = "https://via.placeholder.com/300x300?text=Hantle",
                Description = "Zestaw hantli 2x5kg z powłoką neoprenową",
                IsDiscount = false,
                IdTypeOfProduct = 4
            },
            new Product
            {
                IdProduct = 10,
                Code = "SPORT-002",
                Name = "Mata do jogi",
                Price = 59.99m,
                FotoURL = "https://via.placeholder.com/300x300?text=Mata",
                Description = "Antypoślizgowa mata do jogi, grubość 6mm, z torbą transportową",
                IsDiscount = true,
                IdTypeOfProduct = 4
            },
            // Książki
            new Product
            {
                IdProduct = 11,
                Code = "BOOK-001",
                Name = "Programowanie w C#",
                Price = 79.99m,
                FotoURL = "https://via.placeholder.com/300x300?text=Ksiazka+CSharp",
                Description = "Kompletny przewodnik po programowaniu w języku C# dla początkujących i zaawansowanych",
                IsDiscount = false,
                IdTypeOfProduct = 5
            },
            new Product
            {
                IdProduct = 12,
                Code = "BOOK-002",
                Name = "ASP.NET Core w praktyce",
                Price = 89.99m,
                FotoURL = "https://via.placeholder.com/300x300?text=Ksiazka+ASPNET",
                Description = "Praktyczny poradnik tworzenia aplikacji webowych w ASP.NET Core MVC",
                IsDiscount = true,
                IdTypeOfProduct = 5
            }
        );
    }
}