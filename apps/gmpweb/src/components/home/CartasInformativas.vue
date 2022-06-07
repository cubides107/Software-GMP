<template>
  <div>
    
    <b-card-group deck >
      <b-col sm="12" md="6" lg="6" xl="3" :key="cartaInformativa.titulo" v-for="cartaInformativa in ListCartasInformativas">
        <b-card :title="cartaInformativa.Titulo" align="center" class="m-3">
          <b-card-text>
            {{ cartaInformativa.descripcion }}
          </b-card-text>
        </b-card>
      </b-col>
    </b-card-group>
    
  </div>
</template>

<script>
import { ListCartasInformativas } from "@/assets/enums/HelpText.js";

export default {
  name: "CartasInformativas",
  
  computed: {
    ListCartasInformativas() {
      return ListCartasInformativas;
    },
    
  },
};
</script>
  
<style>
.card-deck {
  margin-top: 40px;
}
</style>