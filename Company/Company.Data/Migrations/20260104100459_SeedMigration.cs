using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Company.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "News",
                columns: new[] { "IdNews", "Content", "DisplayOrder", "LinkTitle", "Title" },
                values: new object[,]
                {
                    { 1, "<p>Zapraszamy na wielką wyprzedaż zimową! Rabaty do 50% na wybrane produkty. Promocja trwa do końca miesiąca.</p><p>Nie przegap okazji!</p>", 1, "Wielka wyprzedaż", "Wielka wyprzedaż zimowa!" },
                    { 2, "<p>Z przyjemnością informujemy, że nowa kolekcja produktów jest już dostępna w naszym sklepie.</p><p>Sprawdź najnowsze trendy i wybierz coś dla siebie!</p>", 2, "Nowa kolekcja", "Nowa kolekcja już dostępna" },
                    { 3, "<p>Miło nam poinformować, że od teraz oferujemy darmową dostawę przy zamówieniach powyżej 100 zł.</p><p>Zamów już dziś i skorzystaj z promocji!</p>", 3, "Darmowa dostawa", "Darmowa dostawa od 100 zł" },
                    { 4, "<p>W okresie świątecznym pracujemy w zmienionych godzinach:</p><ul><li>24.12 - 8:00 - 12:00</li><li>25-26.12 - zamknięte</li><li>31.12 - 8:00 - 14:00</li></ul>", 4, "Godziny świąteczne", "Zmiana godzin pracy" }
                });

            migrationBuilder.InsertData(
                table: "Page",
                columns: new[] { "IdPage", "Content", "DisplayOrder", "LinkTitle", "Title" },
                values: new object[,]
                {
                    { 1, "<p>Jesteśmy firmą z wieloletnim doświadczeniem na rynku. Specjalizujemy się w dostarczaniu najwyższej jakości produktów i usług dla naszych klientów.</p><p>Nasza misja to zapewnienie satysfakcji każdego klienta poprzez profesjonalną obsługę i konkurencyjne ceny.</p>", 1, "O nas", "O naszej firmie" },
                    { 2, "<p><strong>Adres:</strong> ul. Przykładowa 123, 00-001 Warszawa</p><p><strong>Telefon:</strong> +48 123 456 789</p><p><strong>Email:</strong> kontakt@firma.pl</p><p>Zapraszamy od poniedziałku do piątku w godzinach 8:00 - 16:00</p>", 2, "Kontakt", "Skontaktuj się z nami" },
                    { 3, "<h3>§1 Postanowienia ogólne</h3><p>Niniejszy regulamin określa zasady korzystania ze sklepu internetowego.</p><h3>§2 Składanie zamówień</h3><p>Zamówienia można składać 24 godziny na dobę, 7 dni w tygodniu.</p><h3>§3 Płatności</h3><p>Akceptujemy płatności kartą, przelewem oraz za pobraniem.</p>", 3, "Regulamin", "Regulamin sklepu" },
                    { 4, "<p>Szanujemy Twoją prywatność. Wszystkie dane osobowe są przetwarzane zgodnie z RODO.</p><p>Twoje dane wykorzystujemy wyłącznie w celu realizacji zamówień i nie udostępniamy ich podmiotom trzecim.</p>", 4, "Polityka prywatności", "Polityka prywatności" }
                });

            migrationBuilder.InsertData(
                table: "TypeOfProduct",
                columns: new[] { "IdTypeOfProduct", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Smartfony, tablety, laptopy i inne urządzenia elektroniczne", "Elektronika" },
                    { 2, "Koszulki, spodnie, kurtki i inne ubrania", "Odzież" },
                    { 3, "Meble, dekoracje, narzędzia ogrodowe", "Dom i ogród" },
                    { 4, "Sprzęt sportowy, odzież sportowa, akcesoria fitness", "Sport" },
                    { 5, "Literatura, podręczniki, audiobooki", "Książki" }
                });

            migrationBuilder.InsertData(
                table: "Product",
                columns: new[] { "IdProduct", "Code", "Description", "FotoURL", "IdTypeOfProduct", "IsDiscount", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "ELEC-001", "Nowoczesny smartfon z ekranem AMOLED 6.5\", 128GB pamięci, aparat 48MP", "https://via.placeholder.com/300x300?text=Smartfon", 1, true, "Smartfon Galaxy X", 2499.99m },
                    { 2, "ELEC-002", "Wydajny laptop z procesorem i7, 16GB RAM, dysk SSD 512GB", "https://via.placeholder.com/300x300?text=Laptop", 1, false, "Laptop ProBook 15", 3999.00m },
                    { 3, "ELEC-003", "Bezprzewodowe słuchawki z aktywną redukcją szumów, 30h pracy", "https://via.placeholder.com/300x300?text=Sluchawki", 1, true, "Słuchawki Bluetooth", 299.99m },
                    { 4, "CLOTH-001", "Wygodna koszulka z bawełny organicznej, dostępna w wielu kolorach", "https://via.placeholder.com/300x300?text=Koszulka", 2, false, "Koszulka Basic", 49.99m },
                    { 5, "CLOTH-002", "Klasyczne jeansy o dopasowanym kroju, wysoka jakość denimu", "https://via.placeholder.com/300x300?text=Jeansy", 2, true, "Jeansy Slim Fit", 149.99m },
                    { 6, "CLOTH-003", "Ciepła kurtka zimowa z kapturem, wodoodporna, rozmiary S-XXL", "https://via.placeholder.com/300x300?text=Kurtka", 2, false, "Kurtka zimowa", 399.00m },
                    { 7, "HOME-001", "Elegancka lampa stołowa LED z regulacją jasności", "https://via.placeholder.com/300x300?text=Lampa", 3, false, "Lampa stołowa LED", 129.00m },
                    { 8, "HOME-002", "Stylowa doniczka ceramiczna, idealna na kwiaty doniczkowe", "https://via.placeholder.com/300x300?text=Doniczka", 3, true, "Doniczka ceramiczna", 39.99m },
                    { 9, "SPORT-001", "Zestaw hantli 2x5kg z powłoką neoprenową", "https://via.placeholder.com/300x300?text=Hantle", 4, false, "Hantle 2x5kg", 89.99m },
                    { 10, "SPORT-002", "Antypoślizgowa mata do jogi, grubość 6mm, z torbą transportową", "https://via.placeholder.com/300x300?text=Mata", 4, true, "Mata do jogi", 59.99m },
                    { 11, "BOOK-001", "Kompletny przewodnik po programowaniu w języku C# dla początkujących i zaawansowanych", "https://via.placeholder.com/300x300?text=Ksiazka+CSharp", 5, false, "Programowanie w C#", 79.99m },
                    { 12, "BOOK-002", "Praktyczny poradnik tworzenia aplikacji webowych w ASP.NET Core MVC", "https://via.placeholder.com/300x300?text=Ksiazka+ASPNET", 5, true, "ASP.NET Core w praktyce", 89.99m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "IdNews",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "IdNews",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "IdNews",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "News",
                keyColumn: "IdNews",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Page",
                keyColumn: "IdPage",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Page",
                keyColumn: "IdPage",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Page",
                keyColumn: "IdPage",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Page",
                keyColumn: "IdPage",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Product",
                keyColumn: "IdProduct",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "TypeOfProduct",
                keyColumn: "IdTypeOfProduct",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "TypeOfProduct",
                keyColumn: "IdTypeOfProduct",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "TypeOfProduct",
                keyColumn: "IdTypeOfProduct",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "TypeOfProduct",
                keyColumn: "IdTypeOfProduct",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "TypeOfProduct",
                keyColumn: "IdTypeOfProduct",
                keyValue: 5);
        }
    }
}
