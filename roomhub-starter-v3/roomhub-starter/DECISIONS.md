# DECISIONS

Answer **yourself**, in your own words, in 3–5 sentences per question.
Refer to concrete files and classes from your own repo (for example `Features/Bookings/CreateBooking/...`).
An unanswered question gives 0 points.

## 1. A rule you moved
Pick one business rule (R1–R8). In which file did it live in the starter and in which file does it live now? Why there?

R7 suprapunerea era în Services/BookingService.cs, în bucla din Create, cu <=, și verifica toate rezervările, inclusiv pe cele anulate. Acum e în Domain/Bookings/Booking.cs iar metoda OverlapsWith folosește < strict și ia în calcul doar rezervările Active, iar Booking.Create o apelează pe lista primită. am pus-o în domeniu pentru ca e o regulă de business despre rezervări nu despre HTTP sau bază de date.

## 2. Result instead of an exception
Why is `Result` a better choice than an exception for "the room is occupied"? What happens in your code when an `Error` is returned?

O camera ocupataă e un rezultat previzibil al unei operații normale, nu o situație excepțională. Cu Result<T> faptul ca operația poate eșua apare în semnătură, iar compilatorul și cititorul îl văd. Si nu te bazezi pe un middleware care s-o prindă. când o regulă nu e respectată domeniul sau handler-ul întoarce un Error, handler-ul îl dă mai departe fără să salveze nimic, iar endpoint-ul îl transformă cu ToProblem() într-un răspuns


## 3. Who knows about EF Core?
Which classes in your project know about EF Core and which do not? What do you gain and what do you pay with Repository + Unit of Work?

doar clasele din Infrastructure/Persistence: RoomHubDbContext, RoomConfiguration BookingConfiguration RoomRepository și BookingRepository. Si Program.cs care le înregistrează. Domain și Features depind doar de interfețele IRoomRepository, IBookingRepository și IUnitOfWork definite în Domain. Caștig domeniul și handler-ele nu se schimbă dacă înlocuiesc SQLite sau EF, iar handler-ele pot fi testate cu implementări false, dar am mai multe fișiere 


## 4. The flow of a request
Draw in text (ASCII arrows) the path of a `POST /api/rooms/{roomId}/bookings` request through your project, with the **real** names of files and classes, from the endpoint to the database.

POST /api/rooms/{roomId}/bookings -> CreateBookingEndpoint ( ia datele din request) -> CreateBookingHandler (cautaa camera și rezervările active si ia ora curentă) -> Booking.Create (verifică regulile R2–R7) -> BookingRepository.Add + RoomHubDbContext.SaveChangesAsync -> sQLite (roomhub.db)


## 5. What you rejected from what the AI gave you
Give a concrete example of code or an idea proposed by the AI that you corrected or rejected, and say why.

AI ul a propus la început să lucrez pe niveluri în ordinea din cerință. La nivelul A, regulile ar fi stat temporar în handlere și datele în memorie iar la B și C le aș fi mutat. Am respins planul, pentru că aș fi scris aceleași reguli de mai multe orisi am construit direct structura finala

## 6. If you had 30 more minutes
What would you improve and what risk did you leave in the project?

Aș scrie teste automate pentru Booking.Create cu un FakeTimeProvider. Aș folosi migrații EF în loc de EnsureCreated. Riscul rămas e că două cereri simultane pot rezerva același interval, pentru că verificarea și salvarea nu se fac într-o tranzacție.
Ai recomandase sa facem si datele cu offset dar nu am implementat.