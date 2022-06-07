<template>
  <div>
    <div id="banner">
      <h1>{{ tituloAcercade }}</h1>
      <h3>{{ subtituloAcercadeLema }}</h3>
    </div>

    <div id="descripcionEmpresa">
      <p>
        {{ descripcionEmpresa }}
      </p>
    </div>

    <b-card-group deck>
      <b-col sm="12"  :md="aboutListCard.id == 2 ? '12' : '6'"  xl="4" :key="aboutListCard.id" v-for="aboutListCard in aboutListCards">
            <b-card :title="aboutListCard.titulo" align="center" class="m-3">
              <b-card-text>
                <b-icon scale="2" :icon="aboutListCard.icon"></b-icon>
                {{ aboutListCard.descripcion }}
              </b-card-text>
            </b-card>
      </b-col>
    </b-card-group>
  </div>
</template>

<script>
import { AboutListCards } from "@/assets/enums/HelpText.js";
import { aboutEmpresaHelp } from "@/assets/enums/HelpText.js";

export default {
  name: "Acercade",

  computed: {
    tituloAcercade() {
      return aboutEmpresaHelp.titulo;
    },
    subtituloAcercadeLema() {
      return aboutEmpresaHelp.subtituloLemaEmpresa;
    },
    descripcionEmpresa() {
      return aboutEmpresaHelp.descripcionEmpresa;
    },
    aboutListCards() {
      return AboutListCards;
    },
    
  },
};
</script>

<style scoped>
.b-icon {
  margin: 0px 20px;
}
#banner h1 {
  font-size: 7vw;
}
#banner {
  color: white;
  display: flex;
  align-items: center;
  flex-direction: column;
  justify-content: center;
  height: 30vh;
  background-color: #343a40;
}
#banner h3 {
  font-size: 2vw;
}
.card {
  border: none;
}

#descripcionEmpresa {
  text-align: justify;
  margin: 30px 10% 0px 10%;
}
</style>