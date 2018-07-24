import "font-awesome/css/font-awesome.min.css";
import "bootstrap/dist/css/bootstrap.min.css";
import "datatables.net-bs4/css/dataTables.bootstrap4.css";
import "fullcalendar/dist/fullcalendar.min.css";
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