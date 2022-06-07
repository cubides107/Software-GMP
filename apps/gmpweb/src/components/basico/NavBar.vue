<template>
  <div>
    <b-navbar toggleable type="dark" variant="dark">
      
      <b-navbar-brand :to="{ name: 'Home' }" class="ms-3"><b-icon icon="house-door"></b-icon><small> GMP</small></b-navbar-brand>
      
      <b-navbar-toggle target="navbar-toggle-collapse">
        <template #default="{ expanded }">
          <b-icon v-if="expanded" icon="chevron-bar-up"></b-icon>
          <b-icon v-else icon="three-dots" animation="cylon"></b-icon>
        </template>
      </b-navbar-toggle>

      <b-navbar-brand :to="{ name: 'OpcionesUsuario' }" class="ml-3"><b-icon icon="person"></b-icon></b-navbar-brand>

      <b-collapse id="navbar-toggle-collapse" is-nav>
        <b-navbar-nav class="m-3">

          <b-nav-item :to="{ name: 'Solicitud' }"><b-icon icon="clipboard-minus"></b-icon> Solicitud</b-nav-item>

          <b-nav-item><b-icon icon="check-circle"></b-icon> Otros</b-nav-item>

          <b-nav-item class="" disabled>Disabled</b-nav-item>
        </b-navbar-nav>
      </b-collapse>
    </b-navbar>

    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: "NavBar",
};
</script>

<style>
</style>