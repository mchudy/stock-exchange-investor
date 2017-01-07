## Źródła danych
- [Stooq](http://stooq.pl/q/d/?s=wig20)
- [GPW](https://www.gpw.pl/notowania_archiwalne)

## Job
Job znajduje się w projekcie `StockExchange.Task.App`. Obsługuje następujące komendy:
- `sync-data`
- `sync-today-data`
- `sync-data-gpw`
- `sync-today-data-gpw`
- `fix-data`
- `help`

Komendy `sync-data` oraz `sync-data-gpw` przyjmują opcjonalne parametry `startDate` (domyślnie 20060101) oraz `endDate` (domyślnie dzisiaj). Oba parametry muszą być datami
w formacie `yyyyMMdd`. Przykład wywołania:
```
.\StockExchange.Task.App.exe sync-data-gpw 20140101 20140228
```

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

## Frontend
Biblioteki frontendowe instalujemy przez **Bowera**. Można to zrobić z konsoli `bower install --save nazwa_pakietu`, dopisując do pliku `bower.json` (jest Intellisense) lub z VS (prawym na bower.json -> Manage Bower Packages).

### Toast notifications
Toasty można pokazywać z poziomu kontrolera (metoda `ShowNotification`) lub z JS `toastr.error("title", "message")`. Oprócz `error` jest też `success`, `info` i `warning`.