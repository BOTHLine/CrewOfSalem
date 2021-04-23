# CrewOfSalem

Diese Mod versucht (alle) Rollen aus dem Spiel *Town of Salem* in Among Us zu bringen. Das Ziel hierbei ist, den Rollen so originalgetreu wie nur möglich zu sein. Natürlich ist es nicht immer zu 100% möglich, da sich das Spielprinzip doch sehr stark von Among Us unterscheidet.  
  
In Town Of Salem gibt es insgesamt 4 verschiedene Fraktionen: Town, Mafia, Neutral, Coven.  
Diese Fraktionen lassen sich noch einmal in ihre Gesinnungen unterscheiden (Investigative, Killing, Benign etc.).
Jede Rolle gehört einer Fraktion sowie einer untergeordneten Gesinnung zu. (Dies ist eigentlich nur für die Lobby Einstellungen relevant, da man dort die Zusammensetzung der mitspielenden Rollen einstellen kann).

## Crew

### Crew Investigate

#### Lookout
Kann wichtige Informationen von überall auf der Karte abrufen (Admin, Vitals, Cams).

#### Psychic
Erhält bei jedem Body Report abwechselnd 3 oder 2 Namen von noch lebenden Mitspielenr (Sich selbst ausgeschlossen).  
Erhält der Psychic 3 Namen, so ist MINDESTENS einer davon böse.  
Erhält der Psychic 2 Namen, so ist MINDESTENS einer davon gut.  
Gut sind alle Mitglieder der Crew sowohl Neutral Benign (Amnesiac, Guardian Angel, Survivor).  

*A vision revealed that X or Y is good!*  
*The crew is too evil to find anyone good!*	- Wenn der Psychic der einzig gute Spieler ist  
*A vision revealed that X, Y, or Z is evil!*  
*The crew is too small to accurately find an evildoer!*	- Wenn weniger als 3 Spieler leben  

#### Sheriff
Erhält extra Informationen, wenn er Leichen reportet. Je schneller, desto besser.
Die zusätzlichen Informationen können sein:

*The victim was killed X seconds ago.*  
*The killer has a Lighter/Darker color.*  
*The victim was a(n) Rolle.*  
*The killer's name has the letter X.*  
*The killer has already killed a total of X players.*	 - Aktueller Kill inklusive  
*The killer was a(n) Rolle.*  

Lighter Color:	Lime, Orange, Pink, Teal, Yellow, White  
Darker Color:	Green, Grey, Brown, Red, Blue, Purple  

#### Spy
Kann venten und den Mafia internen Chat mitlesen.

### Crew Killing

#### Veteran
Solange der Veteran auf **Alert** ist, tötet er automatisch jeden Spieler, der in den nächsten X Sekunden versucht eine Fähigkeit auf ihn zu nutzen (Egal ob positiv oder negativ).

#### Vigilante
Kann auf andere Spieler schießen. Sollte er einen Spieler der Crew erwischen, stirbt er selber. Ansonsten wird sein Ziel getötet.

### Crew Protective

#### Bodyguard
Kann sein **Guard** aktivieren und deaktivieren. Während diese Fähigkeit aktiviert ist, kann kein Spieler in seiner Nähe getötet werden. Sollte ein Spieler dies versuchen, sterben stattdessen der Angreifer und der Bodyguard.  
Der Bodyguard kann sich nicht selber schützen.  
Der Bodyguard kann andere Spieler nicht beschützen, während er selber "beschäftigt" ist. (In einer Task, in Kameras, am Admin Table etc..)

#### Doctor
Kann einem anderen Spieler einen Schild geben, welcher bis zum nächsten Meeting hält. Der erste Angriff auf das geschildete Ziel macht den Schild kaputt.
Sobald der Schild weg ist (durch Meeting oder Angriff), beginnt der Cooldown für die Schild-Fähigkeit des Doctors. 
Wenn der Doctor stirbt, verschwindet der Schild nicht, da dieser auf andere Art und Weise bricht.

### Crew Support

#### Escort
Kann einen anderen Spieler "blockieren" und dadurch den Cooldown aller seiner Fähigkeiten erhöhen. Zusätzlich kann dieser Spieler für die selbe Dauer keine Vents benutzen.

#### Mayor
Kann sich selber jederzeit innerhalb einer Runde revealen. Ab dem nächsten Meeting ist dies auch für alle anderen Spieler ersichtlich (Name und Rolle werden angezeigt).
Sobald der Mayor sich revealed hat, zählen seine Votes in einem Meeting doppelt so viel.

#### Medium
Das Medium kann seine Seance nutzen, um für eine gewisse Zeit alle Geister zu sehen.

## Mafia

### Mafia Deception

#### Disguiser
Kann alle Spieler für X Sekunden grau erscheinen lassen und alle Namen verschleiern.

#### Hypnotist
Kann einen Spieler pro runde "hypnotizen". Diese Person nimmt in der Runde nach dem nächsten Meeting das Aussehen und die Namen aller Spieler willkürlich vertauscht wahr.

### Mafia Killing

#### Ambusher
Kann töten, ohne dabei auf die Leiche zu springen. Kann außerdem aus einem Vent heraus töten, ohne diesen zu verlassen.

#### Forger
Kann von anderen Spielern das Aussehen klauen und sich anschließend in diesen verwandeln.

### Mafia Support

#### Blackmailer
Kann einen Spieler pro Runde "blackmailen". Diese Person darf in dem nächsten Meeting nichts sagen/schreiben und kann nicht voten.

#### Consigliere
Kann einen Spieler untersuchen und die exakte Rolle herausfinden.

#### Consort
Kann einen anderen Spieler "blockieren" und dadurch den Cooldown aller seiner Fähigkeiten erhöhen. Zusätzlich kann dieser Spieler für die selbe Dauer keine Tasks machen.

## Neutral

### Neutral Benign

#### Guardian Angel
Bekommt am Anfang der Runde ein Ziel zuegewiesen, welches jede Rolle außer Guardian Angel, Executioner oder Jester sein kann. Der Guardian Angel gewinnt, solange sein Ziel bis zum Ende überlebt, auch wenn er selber bereits tot ist.  
Kann sein Ziel für X Sekunden schützen. Alle Kill-Versuche innerhalb dieser Zeit auf das Ziel des Guardian Angels bringen nur den Kill auf Cooldown, töten es aber nicht. Kann auch aus dem Tod heraus genutzt werden.  
Sollte das Ziel vor dem Guardian Angel sterben, so wird der Guardian Angel zum Survivor.

#### Survivor
Möchte nur bis zum Ende überleben. Kann sowohl mit der Crew als auch mit der Mafia gewinnen, solange er bis zum Ende überlebt. Außer wenn die Mafia durch Sabotage (Reactor, Oxygen) gewinnt.  
Kann sich selber für X Sekunden schützen. Alle Kill-Versuche innerhalb dieser Zeit auf den Survivor bringen nur den Kill auf cooldown, töten ihn aber nicht.  
Der Survivor macht keine Tasks.

### Neutral Evil

#### Executioner
Bekommt am Anfang der Runde ein Ziel zugewiesen, welches immer Teil der Crew ist. Der Executioner gewinnt alleine, sobald dieses Ziel durch ein Meeting raus gevotet wird.  
Sollte das Ziel auf eine andere Art und Weise sterben, so wird der Executioner zum Jester.  
Der Executioner macht keine Tasks.

#### Jester
Gewinnt alleine, wenn er selber durch ein Meeting raus gevotet wird.  
Der Jester macht keine Tasks.