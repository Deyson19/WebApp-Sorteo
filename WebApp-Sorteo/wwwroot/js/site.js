//*Obtener la tabla con id=tblDatos sin importar que vista sea. Script global
const table = document.querySelector("#tblDatos");
const tableClass = [
  "table-bordered",
  "table-hover",
  "table-inverse",
  "table-dark",
  "align-middle",
  "table-responsive",
  "shadow-lg",
];
if (table) {
  tableClass.forEach((x) => {
    table.classList.add(x);
  });
  $(document).ready(function () {
    $("#tblDatos").DataTable({
      language: {
        url: "https://cdn.datatables.net/plug-ins/9dcbecd42ad/i18n/Spanish.json",
      },
      aLengthMenu: [
        [15, 50, 100, 200, -1],
        [15, 50, 100, 200, "Todos"],
      ],
    });
  });
}
