<script setup>
import { onBeforeMount, watch } from "vue";
import { useRouter } from "vue-router";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import Dropdown from "primevue/dropdown";
import Calendar from "primevue/calendar";
import { useToast } from "primevue/usetoast";

import { PRIMARY_CITIES } from "../../constants";

// *** Mock data ***
const fectchData = {
    name: "Health and Wellbeing at work",
    location: {
        city: "Cần Thơ",
        address: "Can Tho University",
    },
    startDate: new Date("02/11/2022").getTime().toString(),
    duration: 300,
    detail: "Lorem ipsum dolor sit amet, consectetur adipisicing elit. Eligendi esse porro odio ea doloribus quaerat iste quae reprehenderit asperiores animi.",
};
// *** End mock data ***

const router = useRouter();
const { _id, eventData } = defineProps({
    _id: String,
    eventData: String,
});

let formData = $ref({
    name: null,
    location: {
        city: null,
        address: null,
    },
    startDate: new Date(),
    duration: 1,
    detail: null,
    posterImg: null,
});

onBeforeMount(() => {
    // Check if this is a Event Edit page
    // Fetch data if there is no passed props
    const { name } = router.currentRoute.value;
    if (name === "Event Edit" && _id) {
        formData = eventData ? JSON.parse(eventData) : fectchData;
    }
});

const toast = useToast();
let submitting = $ref(false);
const submitData = () => {
    submitting = true;

    setTimeout(() => {
        submitting = false;

        toast.add({
            severity: "success",
            summary: "Successful",
            detail: "New event is created",
            life: 3000,
        });

        router.push({ name: "Events Management" });
    }, 2000);
};
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <h3 class="title">Blood Donations Events Creation Forms</h3>

                <!-- Form -->
                <div class="p-fluid formgrid grid">
                    <!-- Event Name -->
                    <div class="field col-12">
                        <label for="event-name">Event Name</label>
                        <InputText
                            v-model="formData.name"
                            id="event-name"
                            type="text"
                        />
                    </div>

                    <!-- Start Date -->
                    <div class="field col-12 md:col-6">
                        <label>Pick the start date</label>
                        <Calendar
                            v-model="formData.startDate"
                            dateFormat="dd/mm/yy"
                            :placeholder="formData.startDate"
                        />
                    </div>

                    <!-- Duration -->
                    <div class="field col-12 md:col-6">
                        <label for="duration">Duration</label>
                        <InputNumber
                            id="duration"
                            v-model="formData.duration"
                            :min="1"
                            :show-buttons="true"
                            :suffix="formData.duration === 1 ? ` day` : ` days`"
                        />
                    </div>

                    <!-- City -->
                    <div class="field col-12 md:col-6">
                        <label for="city">City</label>
                        <Dropdown
                            id="city"
                            v-model="formData.location.city"
                            :options="PRIMARY_CITIES"
                            placeholder="Select One"
                        ></Dropdown>
                    </div>

                    <!-- Address -->
                    <div class="field col-12 md:col-6">
                        <label for="address">Address</label>
                        <InputText
                            id="address"
                            type="text"
                            v-model="formData.location.address"
                        />
                    </div>

                    <!-- Event Poster -->
                    <div class="field col-12">
                        <label for="poster">Event poster (.png, .jpg)</label>
                        <InputText
                            id="poster"
                            type="file"
                            v-model="formData.posterImg"
                            accept="image/png, image/gif, image/jpeg"
                        />
                    </div>

                    <!-- Submiitting button -->
                    <div class="field col-12">
                        <PrimeVueButton
                            type="button"
                            label="Submit"
                            class="submit-btn mb-2 mr-2"
                            @click="submitData"
                            :loading="submitting"
                        />
                    </div>
                </div>

                <br />
                <br />

                {{ formData }}
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.title {
    font-weight: 900;
    color: var(--primary-color);
    text-align: center;
    margin-bottom: 2rem;
}

.submit-btn {
    margin-top: 3em;
    width: 8em;
}
</style>
