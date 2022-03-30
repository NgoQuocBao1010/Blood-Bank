<script setup>
import { onBeforeMount, reactive } from "vue";
import { useRouter } from "vue-router";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import Textarea from "primevue/textarea";
import Dropdown from "primevue/dropdown";
import Calendar from "primevue/calendar";
import { useToast } from "primevue/usetoast";
import useVuelidate from "@vuelidate/core";
import { required } from "@vuelidate/validators";

import { useEventStore } from "../../stores/event";
import EventRepo from "../../api/EventRepo";
import { PRIMARY_CITIES } from "../../constants";

const router = useRouter();
const isEditPage = $computed(
    () => router.currentRoute.value.name === "Event Edit"
);

const { _id, eventData } = defineProps({
    _id: String,
    eventData: String,
});

let formData = $ref({
    name: "",
    location: {
        city: "",
        address: "",
    },
    startDate: new Date(),
    duration: 1,
    detail: "",
    posterImg: null,
});
const formRules = $computed(() => {
    return {
        name: { required },
        duration: { required },
        location: {
            city: { required },
            address: { required },
        },
        detail: { required },
    };
});

onBeforeMount(async () => {
    // Check if this is a Event Edit page
    // Fetch data if there is no passed props
    if (isEditPage && _id) {
        if (eventData) {
            fixingVuevalidateBugs(JSON.parse(eventData));
        } else {
            const { data } = await EventRepo.getById(_id);
            fixingVuevalidateBugs(data);
        }
    }
});

const $v = $(useVuelidate(formRules, formData));
const toast = useToast();
let submitting = $ref(false);
const submitData = async () => {
    // Form validation
    const isCorrect = await $v.$validate();
    if (!isCorrect) {
        toast.add({
            severity: "error",
            summary: "Form Error",
            detail: "Please fix your form 🙏",
            life: 3000,
        });

        return;
    }
    // Make API call to server
    submitting = true;
    formData.startDate = formData.startDate.getTime().toString();

    if (isEditPage) {
        setTimeout(() => {
            submitting = false;

            toast.add({
                severity: "success",
                summary: "Successful",
                detail: "New event is created",
                life: 3000,
            });

            // router.push({ name: "Events Management" });
        }, 800);
    } else {
        const { data, status } = await EventRepo.createNewEvent(formData);

        if (data && status === 200) {
            await useEventStore().setEvents();
            toast.add({
                severity: "success",
                summary: "Successful",
                detail: "New event is created",
                life: 3000,
            });

            router.push({ name: "Events Management" });
            submitting = false;
        }
    }
};

// Helpers
const fixingVuevalidateBugs = (data) => {
    formData.name = data.name;
    formData.detail = data.detail;
    formData.location.city = data.location.city;
    formData.location.address = data.location.address;
    formData.duration = data.duration;
    formData.startDate = new Date(parseInt(data.startDate));
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
                            :class="{ 'p-invalid': $v.name.$error }"
                        />
                        <span v-if="$v.name.$error" class="app-form-error">
                            This field is required
                        </span>
                    </div>

                    <!-- Event detail -->
                    <div class="field col-12">
                        <label for="event-detail">Event description</label>
                        <Textarea
                            v-model="formData.detail"
                            :autoResize="true"
                            rows="5"
                            cols="30"
                            :class="{ 'p-invalid': $v.detail.$error }"
                        />
                        <span v-if="$v.detail.$error" class="app-form-error">
                            This field is required
                        </span>
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
                            :class="{ 'p-invalid': $v.duration.$error }"
                        />
                        <span v-if="$v.duration.$error" class="app-form-error">
                            This field is required
                        </span>
                    </div>

                    <!-- City -->
                    <div class="field col-12 md:col-6">
                        <label for="city">City</label>
                        <Dropdown
                            id="city"
                            v-model="formData.location.city"
                            :options="PRIMARY_CITIES"
                            placeholder="Select One"
                            :class="{ 'p-invalid': $v.location.city.$error }"
                        ></Dropdown>
                        <span
                            v-if="$v.location.city.$error"
                            class="app-form-error"
                        >
                            This field is required
                        </span>
                    </div>

                    <!-- Address -->
                    <div class="field col-12 md:col-6">
                        <label for="address">Address</label>
                        <InputText
                            id="address"
                            type="text"
                            v-model="formData.location.address"
                            :class="{ 'p-invalid': $v.location.address.$error }"
                        />
                        <span
                            v-if="$v.location.address.$error"
                            class="app-form-error"
                        >
                            This field is required
                        </span>
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
