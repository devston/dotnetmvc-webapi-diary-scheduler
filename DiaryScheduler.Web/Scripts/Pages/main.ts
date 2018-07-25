import "@fortawesome/fontawesome-free-webfonts/css/fontawesome.css";
import "@fortawesome/fontawesome-free-webfonts/css/fa-solid.css";
import "@fortawesome/fontawesome-free-webfonts/css/fa-regular.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "datatables.net-bs4/css/dataTables.bootstrap4.css";
import "Content/style.scss";
import $ from "jquery";
import "bootstrap";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import { Site } from "Scripts/Utilities/site-core";
import "Scripts/Pages/scheduler";

$(document).ready(function () {
    Site.init();
});