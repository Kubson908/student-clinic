<script setup lang="ts">
import HomePageCard from "./HomePageCard.vue";
// Linki do pobrania zdjec z karuzeli
// https://pixabay.com/pl/illustrations/lekarz-szpital-opieki-zdrowotnej-6701410/
// https://pixabay.com/pl/photos/test-tube-covid-19-mask-maska-5065426/
// https://pixabay.com/pl/photos/poziom-cukru-we-krwi-glukometr-r%c4%99ka-3310318/
</script>
<template>
  <v-row no-gutters>
    <v-col>
      <v-row no-gutters>
        <v-col>
          <v-carousel
            show-arrows="hover"
            hide-delimiter-background
            color="white"
            class="elevation-6"
            cycle
          >
            <v-carousel-item>
              <v-parallax
                height="100%"
                src="https://cdn.pixabay.com/photo/2021/10/11/17/37/doctor-6701410_960_720.jpg"
              >
              </v-parallax>
            </v-carousel-item>

            <v-carousel-item>
              <v-parallax
                height="100%"
                src="https://cdn.pixabay.com/photo/2020/04/19/20/10/test-tube-5065426_960_720.jpg"
              >
              </v-parallax>
            </v-carousel-item>

            <v-carousel-item>
              <v-parallax height="100%" src="slider1.png"> </v-parallax>
            </v-carousel-item>
          </v-carousel>
        </v-col>
      </v-row>
      <v-row no-gutters class="hp-bright py-8">
        <v-col>
          <v-container class="hp-width">
            <v-row>
              <v-col class="d-flex justify-md-start">
                <p class="text-uppercase text-h4 text-left">
                  STUDIUJESZ? ZADBAMY O TWOJE ZDROWIE!
                </p>
              </v-col>
            </v-row>
            <v-row>
              <v-col
                class="d-flex justify-md-start justify-center"
                md="4"
                sm="6"
                cols="12"
              >
                <HomePageCard class="hp-dark">
                  <template v-slot:header>
                    <v-card-title>
                      <p class="text-uppercase">Krótkie</p>
                      <p class="text-uppercase">Terminy</p>
                    </v-card-title>
                  </template>
                  Duża dostępność lekarzy i krótkie terminy oczekiwania na
                  wizyty. Możliwość skorzystania z teleporad i wideoporad.
                  Pełna, elektroniczna obsługa procesu skierowań i zwolnień
                  lekarskich.
                </HomePageCard>
              </v-col>
              <v-col
                class="d-flex justify-md-center justify-sm-end justify-center"
                md="4"
                sm="6"
                cols="12"
              >
                <HomePageCard class="hp-dark">
                  <template v-slot:header>
                    <v-card-title>
                      <p class="text-uppercase">Dogodna</p>
                      <p class="text-uppercase">Lokalizacja</p>
                    </v-card-title>
                  </template>
                  Dobrze skomunikowana lokalizacja. Bezpłatny parking, bliskość
                  przystanków autobusowych i tramwajowych.
                </HomePageCard>
              </v-col>
              <v-col
                class="d-flex justify-md-end justify-sm-start justify-center"
                md="4"
                sm="6"
                cols="12"
              >
                <HomePageCard class="hp-dark">
                  <template v-slot:header>
                    <v-card-title>
                      <p class="text-uppercase">Gabinety</p>
                      <p class="text-uppercase">Specjalistyczne</p>
                    </v-card-title>
                  </template>
                  Kilka poradni specjalistycznych. Najwyższej klasy sprzęt
                  medyczny, dostęp do najskuteczniejszej i najnowocześniejszej
                  diagnostyki i terapii.
                </HomePageCard>
              </v-col>
            </v-row>
          </v-container>
        </v-col>
      </v-row>
      <v-row no-gutters class="hp-dark rounded-xl py-8">
        <v-col>
          <v-container class="hp-width">
            <v-row>
              <v-col class="d-flex justify-md-start">
                <p class="text-uppercase text-h4 text-left">Nasi lekarze</p>
              </v-col>
            </v-row>
            <v-row>
              <v-col
                md="4"
                sm="6"
                cols="12"
                class="d-flex justify-md-start justify-center"
              >
                <HomePageCard class="hp-bright">
                  <template v-slot:header>
                    <v-card-title class="text-uppercase text-wrap">
                      Dr Maciej Kowalczyk
                    </v-card-title>
                    <v-card-subtitle> Pulmonolog </v-card-subtitle>
                  </template>
                  Specjalista pulmonolog, którego doświadczenie i wiedza
                  pozwalają na skuteczne zapobieganie, diagnozowanie i leczenie
                  chorób płuc, poprawiając jakość życia pacjentów.
                </HomePageCard>
              </v-col>
              <v-col
                md="4"
                sm="6"
                cols="12"
                class="d-flex justify-md-center justify-sm-end justify-center"
              >
                <HomePageCard class="hp-bright">
                  <template v-slot:header>
                    <v-card-title class="text-uppercase text-wrap">
                      Dr Małgorzata Bęcka
                    </v-card-title>
                    <v-card-subtitle> Okulista </v-card-subtitle>
                  </template>
                  Specjalista w dziedzinie okulistyki z dużym doświadczeniem.
                  Oferuje badania diagnostyczne oraz skuteczne leczenie chorób
                  takich jak katarakta, jaskra czy retinopatia cukrzycowa.
                </HomePageCard>
              </v-col>
              <v-col
                md="4"
                sm="6"
                cols="12"
                class="d-flex justify-md-end justify-sm-start justify-center"
              >
                <HomePageCard class="hp-bright">
                  <template v-slot:header>
                    <v-card-title class="text-uppercase text-wrap">
                      Dr Marcin Mróz
                    </v-card-title>
                    <v-card-subtitle> Internista </v-card-subtitle>
                  </template>
                  Lekarz internista, specjalizujący się w chorobach
                  wewnętrznych. Ukończył studia medyczne na uniwersytecie w
                  Oxfordzie. Wyróżnia go empatyczne podejście do pacjentów.
                </HomePageCard>
              </v-col>

              <v-col
                md="4"
                sm="6"
                cols="12"
                class="d-flex justify-md-start justify-sm-end justify-center"
              >
                <HomePageCard class="hp-bright">
                  <template v-slot:header>
                    <v-card-title class="text-uppercase text-wrap">
                      Dr Anna Malinowska
                    </v-card-title>
                    <v-card-subtitle> Gastrolog </v-card-subtitle>
                  </template>
                  Skutecznie diagnozuje i leczy schorzenia układu pokarmowego.
                  Zaufanie i komfort pacjentów to dla niej priorytet. Oferuje
                  kompleksowe podejście do pacjenta dostosowane do jego potrzeb.
                </HomePageCard>
              </v-col>
              <v-col
                md="4"
                sm="6"
                cols="12"
                class="d-flex justify-md-center justify-sm-start justify-center"
              >
                <HomePageCard class="hp-bright">
                  <template v-slot:header>
                    <v-card-title class="text-uppercase text-wrap">
                      Dr Artur Mazur
                    </v-card-title>
                    <v-card-subtitle> Internista </v-card-subtitle>
                  </template>
                  Internista z doświadczeniem w diagnostyce i leczeniu chorób
                  układu sercowo-naczyniowego oraz chorób wewnętrznych. Zawsze
                  dba o swoich pacjentów i zapewnia im kompleksową opiekę
                  medyczną.
                </HomePageCard>
              </v-col>
            </v-row>
          </v-container>
        </v-col>
      </v-row>
      <v-row no-gutters class="hp-bright py-8">
        <v-col>
          <v-container class="hp-width">
            <v-row>
              <v-col cols="12" md="6" sm="12">
                <v-row>
                  <v-col class="d-flex justify-left">
                    <p class="text-uppercase text-h4">O nas</p>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col class="d-flex justify-md-start justify-center">
                    <HomePageCard min-width="75%" class="hp-dark">
                      <template v-slot:header>
                        <v-card-title
                          class="text-uppercase text-wrap text-left"
                        >
                          Działamy od 2023 roku
                        </v-card-title>
                      </template>
                      Nasza placówka działa już od ponad 2 miesięcy, zawsze z
                      myślą o zdrowiu i dobrej kondycji naszych pacjentów. Nasza
                      misja to zapewnienie kompleksowej opieki medycznej na
                      najwyższym poziomie oraz pomoc w utrzymaniu dobrego stanu
                      zdrowia przez całe życie. Wychodzimy z założenia, że każdy
                      pacjent jest inny i wymaga indywidualnego podejścia,
                      dlatego nasi lekarze dokładają wszelkich starań, by
                      zapewnić wysoką jakość usług i przyjazną atmosferę w
                      trakcie wizyty.
                    </HomePageCard>
                  </v-col>
                </v-row>
              </v-col>
              <v-col md="6" sm="12" cols="12" class="d-flex child-flex">
                <v-img
                  class="rounded-lg"
                  height="100%"
                  cover
                  src="tabs.jpeg"
                ></v-img>
              </v-col>
            </v-row>
          </v-container>
        </v-col>
      </v-row>
      <v-row no-gutters class="hp-dark rounded-xl py-8">
        <v-col>
          <v-container class="hp-width">
            <v-row>
              <v-col cols="12" md="6" sm="12">
                <v-row>
                  <v-col class="d-flex justify-left">
                    <p class="text-uppercase text-h4 text-left">Kontakt</p>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col class="d-flex justify-md-start justify-center">
                    <HomePageCard min-width="75%" class="hp-bright">
                      <p class="font-weight-bold">Skontaktuj się z nami:</p>
                      <p>Studencka przychodnia zdrowia</p>
                      <p>ul. Przykładowa 123</p>
                      <p>00-000 Olsztyn</p>
                      <p>
                        <span class="font-weight-medium">Telefon: </span> +48
                        123 456 789
                      </p>
                      <p>
                        <span class="font-weight-medium">E-mail: </span>
                        recepcja@przychodniaxyz.pl
                      </p>
                      <br />
                      <p class="font-weight-bold">Godziny otwarcia:</p>
                      <p>
                        <span class="font-weight-medium"
                          >Poniedziałek - Piątek:
                        </span>
                        8:00 - 20:00
                      </p>
                      <p>
                        <span class="font-weight-medium">Sobota: </span> 9:00 -
                        14:00
                      </p>
                    </HomePageCard>
                  </v-col>
                </v-row>
              </v-col>
              <v-col class="d-flex child-flex" md="6" sm="12" cols="12">
                <v-img
                  class="rounded-lg fill-height"
                  cover
                  src="https://media.istockphoto.com/id/183796231/photo/newer-white-office-building-from-street-view.jpg?s=612x612&w=0&k=20&c=a2zfzSUbsYzj_bmCc_yw2oTK02T7VJhaypehiQg4Htw="
                ></v-img>
              </v-col>
            </v-row>
          </v-container>
        </v-col>
      </v-row>
      <v-row no-gutters
        ><v-col><v-spacer></v-spacer></v-col
      ></v-row>
    </v-col>
  </v-row>
</template>
<style>
.hp-bright {
  color: var(--primary) !important;
  background-color: var(--secondary) !important;
}
.hp-dark {
  color: var(--secondary) !important;
  background-color: var(--primary) !important;
}
.hp-width {
  width: 75% !important;
}
@media screen and (min-width: 1921px) {
  .hp-width {
    width: 50% !important;
  }
}
</style>
