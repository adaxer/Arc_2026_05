# Modul 05 – Domain Driven Design  
## Online-Buchverleihsystem

## 1 Domänenanalyse

Wichtige fachliche Konzepte des Systems sind:

- Benutzer
- Buch
- Exemplar
- Ausleihe
- Rückgabe
- Ausleihstatus
- Leihfrist
- Verfügbarkeit
- Reservierung
- Mahnungen oder Gebühren

Diese Begriffe beschreiben die zentralen Geschäftsprozesse des Systems.  
Eine wichtige fachliche Unterscheidung besteht zwischen **Buch** und **Exemplar**:  
Ein Buch beschreibt einen Titel im Katalog, während ein Exemplar eine konkret verleihbare Einheit darstellt.

---

## 2 Identifizierung der Entitäten

Folgende Konzepte werden als Entitäten modelliert:

### Benutzer
Ein Benutzer besitzt eine eindeutige Identität und bleibt über die Zeit derselbe, auch wenn sich Eigenschaften wie Name oder Adresse ändern. Daher ist er eine Entität.

### Buch
Ein Buch repräsentiert einen Titel im Katalog (z. B. identifiziert über ISBN oder eine interne ID). Da ein Buch eindeutig identifizierbar ist und dauerhaft im System existiert, wird es als Entität modelliert.

### Exemplar
Ein Exemplar ist eine konkrete Instanz eines Buches, die ausgeliehen werden kann. Mehrere Exemplare können zu einem Buch gehören, jedoch besitzt jedes Exemplar eine eigene Identität und einen eigenen Zustand (z. B. verfügbar oder ausgeliehen).

### Ausleihe
Eine Ausleihe beschreibt einen konkreten Ausleihvorgang zwischen Benutzer und Exemplar. Sie besitzt eigene Attribute wie Ausleihdatum, Fälligkeitsdatum und Rückgabestatus und verändert sich während ihres Lebenszyklus. Daher wird sie ebenfalls als Entität modelliert.

---

## 3 Definition der Value Objects

Einige Konzepte können sinnvoll als Value Objects modelliert werden:

### Leihfrist / Zeitraum
Besteht beispielsweise aus Ausleihdatum und Rückgabedatum.  
Dieses Objekt besitzt keine eigene Identität und wird ausschließlich über seine Werte definiert.

### Adresse
Falls Benutzerdaten im System gespeichert werden, kann eine Adresse als Value Object modelliert werden, da sie nur aus Werten besteht und keine eigene Identität benötigt.

### Ausleihstatus
Der Status einer Ausleihe (z. B. *ausgeliehen*, *zurückgegeben*, *überfällig*) kann als Value Object oder Enumeration modelliert werden, da er lediglich einen Zustand beschreibt.

### Gebührenbetrag
Falls Mahngebühren berechnet werden, kann ein Geldbetrag als Value Object modelliert werden, da hier der Wert und nicht eine Identität relevant ist.

---

## 4 Aggregat Design

Ein mögliches Aggregat im System ist das **Ausleih-Aggregat**.

### Root Entity
Ausleihe

### Bestandteile des Aggregats

- Ausleihe (Root)
- Referenz auf Benutzer
- Referenz auf Exemplar
- Leihfrist
- Ausleihstatus

### Begründung

Die Ausleihe bildet den zentralen Geschäftsprozess des Systems.  
Alle wichtigen fachlichen Regeln hängen mit der Ausleihe zusammen, beispielsweise:

- Ein Exemplar darf nicht mehrfach gleichzeitig ausgeliehen werden.
- Eine Rückgabe beendet eine aktive Ausleihe.
- Der Ausleihstatus muss konsistent bleiben.

Die Root Entity **Ausleihe** stellt sicher, dass diese Regeln innerhalb des Aggregats eingehalten werden.

---

## Fazit

Das Domänenmodell des Online-Buchverleihsystems basiert auf den zentralen Entitäten:

- Benutzer
- Buch
- Exemplar
- Ausleihe

Ergänzend werden Value Objects wie **Leihfrist**, **Adresse**, **Ausleihstatus** oder **Gebührenbetrag** verwendet.

Ein sinnvolles Aggregat ist das **Ausleih-Aggregat** mit **Ausleihe** als Root Entity, da hier die wichtigsten Geschäftsregeln des Systems zusammenlaufen.