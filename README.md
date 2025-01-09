## Ohjelman toiminta

Ohjelma luo Received Files kansion oikeaan paikkaan saapuneista käyttäjä tiedostoista. Valittu kansio luo kansion työpöydälle, taustakuva on png tiedosto joka luodaan työpöydälle ja wifi profiileista ei tule kansiota. Selain tiedot eivät vielä toimi.

Konsolia kannattaa seurata ohjelman edistyessä. Ohjelmalla voi mennä jonkin aikaa ison tiedostojen kasaamisessa ja lähettämisessä.

---
### Ongelmia

Jos peruuttaa lähettämisen, niin pitää sovellus avata uudestaan, kaiken varalta. Ohjelma ei kommunikoi peruuttamista toiselle koneelle, jos ohjelma peruutetaan yhdellä koneella, pitää se sulkea toisellakin.

Jos vastaanottavalla peruutetaan, lähettävä kone yrittää vielä lähettää tiedostot. Tästä ei tule mitään ongelmia, mutta parhain on jos ei tarvitse peruuttaa ohjelmaa.

Jos ohjelman suoritus keskeytyy mistä tahansa syystä, voi jäädä temp tiedostoihin zip kansioita. Lähettävä kone osaa hoitaa nämä itse mutta vastaanottava kone ei välttämättä pysty.

Edistymis palkki ei toimi.

---
### Selain tiedot.

#### **Selain tiedot eivät vielä toimi!!**

Jos selain tietojen kloonamista käytetään, lähettävä kone lähettää kopion, chromen, edgen ja operan Default kansiosta jos ne ovat olemassa. Kansiot jokaiselle selaimelle löytyy käyttäjän temp kansiosta, Browser Data. Jos kyseisten selainten Default kansion tiedostot korvaa tiedostoilla Browser Data kansioista, kaikki muut selaimen tiedot siirtyy paitsi, kirjautumis tiedot ja evästeet. Jotkin laajennukset eivät saata toimia oikein ja voi olla muita ongelmia joista en tiedä.

---
