import * as defaultEventCoverImage from "../assets/images/default_event_cover.png";
import { fileToBase64 } from "../utils";

const THEMES = {
    primaryColor: "#f07279",
    secondaryColor: "#faadb3",
    darkColor: "#1e2d50",
};

const BLOOD_TYPES = ["A", "B", "AB", "O", "Rh"];

const PRIMARY_CITIES = [
    "Cần Thơ",
    "Hồ Chí Minh",
    "Đà Nẵng",
    "Hà Nội",
    "Hậu Giang",
    "An Giang",
    "Đà Lạt",
];

const TRANSACTION_STATUS = ["pending", "failed", "success"];

const DEFAULT_EVENT_COVER = defaultEventCoverImage.default;

export {
    THEMES,
    BLOOD_TYPES,
    PRIMARY_CITIES,
    TRANSACTION_STATUS,
    DEFAULT_EVENT_COVER,
};
