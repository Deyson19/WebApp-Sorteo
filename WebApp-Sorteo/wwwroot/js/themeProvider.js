const themeLightClass = [
  "fa",
  "fa-solid",
  "fa-moon",
  "fa-lg",
  "fa-beat",
  "btn-dark",
];
const themeDarkClass = [
  "fa",
  "fa-regular",
  "fa-sun",
  "fa-lg",
  "fa-beat",
  "btn-light",
];

$(document).ready(function () {
  const myCurrentTheme = localStorage.getItem("theme");
  const iconTheme = document.getElementById("iconTheme");
  const navBar = document.querySelector("#navbar");

  if (myCurrentTheme) {
    document.documentElement.setAttribute("data-bs-theme", myCurrentTheme);
    if (myCurrentTheme === "dark") {
      iconTheme.classList.add(...themeDarkClass);
      const navBarTheme = ["navbar-dark", "bg-dark"];
      navBar.classList.add(...navBarTheme);
    } else {
      iconTheme.classList.add(...themeLightClass);
      const navBarTheme = ["navbar-light", "bg-light"];
      navBar.classList.add(...navBarTheme);
    }
  } else {
    document.documentElement.setAttribute("data-bs-theme", "light");
    iconTheme.classList.add(...themeLightClass);
  }

  document
    .getElementById("btnThemeMode")
    .addEventListener("click", function () {
      applyTheme();
    });

  function applyTheme() {
    var iconTheme = document.getElementById("iconTheme");
    var myCurrentTheme = localStorage.getItem("theme");

    if (myCurrentTheme === "dark") {
      document.documentElement.setAttribute("data-bs-theme", "light");
      iconTheme.classList.remove(...themeDarkClass);
      iconTheme.classList.add(...themeLightClass);
      localStorage.setItem("theme", "light");
    } else {
      document.documentElement.setAttribute("data-bs-theme", "dark");
      iconTheme.classList.remove(...themeLightClass);
      iconTheme.classList.add(...themeDarkClass);
      localStorage.setItem("theme", "dark");
    }

    setTimeout(() => {
      window.location.reload();
    }, 1500);
    toastr.success("Se ha cambiado el tema");
  }
});
