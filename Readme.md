## Teoria

Wszystko oparte jest o wykres świecowy (cena otwarcia, zamknięcia, najniższa, najwyższa danego dnia dla danej spółki).

Stooq to najelpsze darmowe źródło danych (ale są problemy, o czym będzie poniżej).

Linki:
[http://stooq.pl/q/d/?s=wig20]()

## Dane

Było mi bardzo trudno znaleźć dane historyczne, wymyśliłem sobie, że mogą być to dane dla spółek z wig 20 z ostatnich 10 lat. Udało mi się zsynchronizować dane ze stooq, ale po jakimś czasie strona zaczęła mnie blokować, ma jakieś limity, importowałem z csv z url. Tym bardziej nie znalazłem żadnego api.

## Architektura

- Data Access - Entity Framework, wydaje mi się, że to bardzo ciekawa architektura, popatrzcie sobie na IRepository i GenericRepository i użycie tego. Póki co mamy dwie tabele w bazie, Company i Price. Chciałbym się trzymać tej architektury.

- Task.App i Task.Business -> aplikacja konsolowa synchronizująca dane, powinien być to job, który uruchamia się sam codziennie na serwerze i pobiera najnowsze dane. Aplikacje wywołuje się z parametrem, bo nie chcemy mnożyć bez sensu takich samych projektów do synchronizowania danych z różnych źródeł, oraz historycznych z roku, 10 lat, czy tyko dnia dzisiejszego. Chyba taką aplikację jaką job można by puścić na azure nawet.
Architektura też mi się podoba, jest autofac, log4net, nie chciałbym z tego rezygnować. Popatrzcie na refleksje przy ogarnianiu parametrów itp., wydaje się to ciekawe. Chcemy teraz nową aplikację, to dodajemy command i logikę w biznes i gotowe, mamy nowy program.

- Common -> ogólny syf który się przydaje, trzeba to na jakieś foldery podzielić, zwróćcie uwagę na consts (nie będziemy hard kodować tekstów) i IFactory i Factory i ich użycie (banalne ale przydatne).

- Business i Web to zwykła tabela wyświetlająca dane z bazy z filtrami (nie działają filtry kolumnowe) z fajnie zrobioną paginacją i napisałem IQuerable Extension -> znaczny zysk na wydajności. 

Nie ruszony jest frontend, ja tego nie umiem i nie chciało mi się tego ruszać, postaram się doprowadzić do używalności tę tabelę.

## Migracje
Dodawanie nowej migracji
```
PM> new-migration NazwaMigracji
```

Uwaga: jak dodajecie stringi z polskimi znakami trzeba zmienić enkodowanie na UTF8, Visual domyślnie daje wszędzie ASCII :/ No i typy kolumn zawsze `nvarchar` i `N` z przodu stringa.

Uruchamianie z Visuala lub z konsoli:
```
StockExchange.Migrations/bin/Debug/StockExchange.Migrations.exe
```
Bez argumentu puszcza na localdb, z argumentem `prod` na produkcyjną bazę, można też podać jako argument connection string.

Potem się dopisze skrypt do deploymentu, żeby migracje zawsze szły razem z zipem i ustawi connection stringi na azurze/octo, żeby ich nie
trzymać w repo i żeby się nie mieszały.

## Frontend
Biblioteki frontendowe instalujemy przez **Bowera**. Można to zrobić z konsoli `bower install --save nazwa_pakietu`, dopisując do pliku `bower.json` (jest Intellisense) lub z VS (prawym na bower.json -> Manage Bower Packages).