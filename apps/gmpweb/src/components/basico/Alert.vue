<template>
  <div>
    <b-alert
      :show="dismissCountDown"
      :variant="estado"
      @dismissed="dismissCountDown = 0"
      @dismiss-count-down="countDownChanged"
    >
      {{ mensaje }} {{ dismissCountDown }}
    </b-alert>
  </div>
</template>

<script>
export default {
  data() {
    return {
      dismissSecs: 5,
      dismissCountDown: 0,
      mensaje: "",
      estado: "",
    };
  },
  created: function () {
    this.$parent.$on("showAlert", this.showAlert);
  },
  methods: {
    countDownChanged(dismissCountDown) {
      this.dismissCountDown = dismissCountDown;
    },
    showAlert(mensaje, estado) {
      this.mensaje = mensaje;
      this.estado = estado;

      this.dismissCountDown = this.dismissSecs;
    },
  },
};
</script>

<style>
</style>