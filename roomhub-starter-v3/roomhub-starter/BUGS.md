# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R7 (intervale care se ating)	 | Services/BookingService.cs, linia 28 | AI | Comparația de suprapunere trebuie să fie strictă (<, nu <=) |
|R7 / R8 (anularea eliberează intervalul)	 | BookingService.cs, linia 26 + BookingRepository.ListForRoom, linia 31 | AI | La verificarea de suprapunere iau în calcul doar rezervările Active |
| R8 (dublă anulare)   	 | BookingService.cs, liniile 56–60 (Cancel) | AI | verific starea |
| R5 (fără start în trecut)	 | BookingService.Create | AI | m adăugat verificarea în Booking.Create: dacă startUtc < nowUtc, întoarce BookingErrors.StartInPast (400). Ora curentă o ia CreateBookingHandler din TimeProvider, nu din DateTime.UtcNow. |
| R6 (maxim 4 ore)	 | BookingService.Create | AI | Am adăugat în Booking.Create verificarea endUtc - startUtc > MaxDuration (4 ore) care întoarce BookingErrors.TooLong (400). Folosesc > strict, ca o rezervare de exact 4 ore să fie permisă. |
