import moment from "moment";

const formatDate = (date) => {
  return moment.unix(date / 1000).format("DD/MM/YYYY");
};

const determineStatus = (event) => {
  const endDate = moment(event.startDate).add(event.duration, "day");
  if (moment().isAfter(endDate, "day")) {
    return "Passed";
  } else if (moment().isBefore(event.startDate, "day")) {
    return "Up coming";
  }

  return "On going";
};

export { determineStatus, formatDate };
