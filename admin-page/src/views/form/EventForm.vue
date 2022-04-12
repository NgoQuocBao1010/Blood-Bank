<script setup>
import { onBeforeMount } from "vue";
import { useRouter } from "vue-router";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import Textarea from "primevue/textarea";
import Dropdown from "primevue/dropdown";
import Calendar from "primevue/calendar";
import Breadcrumb from "primevue/breadcrumb";
import Toast from "primevue/toast";
import { useToast } from "primevue/usetoast";
import useVuelidate from "@vuelidate/core";
import { required } from "@vuelidate/validators";

import { useEventStore } from "../../stores/event";
import EventRepo from "../../api/EventRepo";
import { fileToBase64 } from "../../utils";
import { PRIMARY_CITIES } from "../../constants";

const router = useRouter();
const isEditPage = $computed(
    () => router.currentRoute.value.name === "Event Edit"
);

// Props
const { _id, eventData } = defineProps({
    _id: String,
    eventData: String,
});

// Breadcums
// Naviagtion settings
const home = $ref({
    icon: "fa-solid fa-calendar-days",
    to: { name: "Events Management" },
});
let items = $ref(null);

// Form data and validation rules
let formData = $ref({
    name: "",
    location: {
        city: "",
        address: "",
    },
    startDate: new Date(),
    duration: 1,
    detail: "",
    binaryImage: null,
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

const onImageUpload = async (e) => {
    const files = e.target.files || e.dataTransfer.files;
    if (!files.length) return;

    formData.binaryImage = await fileToBase64(files[0]);
};

const removeUploadImage = () => {
    formData.binaryImage = null;
    document.getElementById("preview").value = "";
};

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

    // Setup breadcum
    if (isEditPage) {
        items = [
            {
                label: `${formData.name} event`,
                to: { name: "Event Detail", params: { _id } },
            },
            { label: "Edit Form" },
        ];
    } else {
        items = [{ label: "Event Creation Form" }];
    }
});

const $v = $(useVuelidate(formRules, formData));
const toast = useToast();
let loading = $ref(false);
// Create or Edit event
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
    loading = true;
    formData.startDate = formData.startDate.getTime().toString();

    try {
        const { data, status } = isEditPage
            ? await EventRepo.edit(_id, formData)
            : await EventRepo.create(formData);

        if (data && status === 200) {
            await useEventStore().setEvents();
            toast.add({
                severity: "success",
                summary: "Successful",
                detail: isEditPage
                    ? "Event is successfully edited"
                    : "New event was successfully created",
                life: 3000,
            });

            const redirectRoute = isEditPage
                ? { name: "Event Detail", params: { _id } }
                : { name: "Events Management" };
            router.push(redirectRoute);
        }
    } catch (e) {
        console.log("Error", e);
        throw e;
    } finally {
        loading = false;
    }
};

// Delete event
const openConfirm = () => {
    toast.add({
        severity: "warn",
        group: "event-confirm",
    });
};
const deleteEvent = async () => {
    loading = true;
    const { data, status } = await EventRepo.delete(_id);
    loading = false;

    if (data && status === 200) {
        await useEventStore().setEvents();
        toast.add({
            severity: "success",
            summary: "Successful",
            detail: "Event is deleted",
            life: 3000,
        });

        router.push({ name: "Events Management" });
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
    formData.binaryImage = data.binaryImage;
};
</script>

<template>
    <div class="grid">
        <div class="col-12">
            <!-- Navigation -->
            <Breadcrumb
                :home="home"
                :model="items"
                style="margin-bottom: 1rem; border-radius: 15px"
            />

            <div class="col-8"></div>
            <div class="card">
                <h3 class="title" v-if="isEditPage">
                    {{ formData?.name }} Event
                </h3>
                <h3 class="title" v-else>
                    Blood Donations Events Creation Forms
                </h3>

                <!-- Form -->
                <div
                    class="p-fluid formgrid col-10 grid"
                    style="margin: 0 auto; min-width: 700px"
                >
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
                            :min-date="new Date()"
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
                    <div class="field col-12 md:col-6">
                        <label for="poster">Event poster (* image file)</label>
                        <InputText
                            id="preview"
                            type="file"
                            @change="onImageUpload"
                            accept="image/png, image/gif, image/jpeg"
                        />
                    </div>

                    <!-- Preview image -->
                    <div class="field col-12 md:col-6">
                        <label>Preview of Event Cover Image</label>
                        <div class="image-wrapper flex flex-center">
                            <template v-if="formData.binaryImage">
                                <img
                                    :src="formData.binaryImage"
                                    class="image-preview"
                                    alt="Preview Image"
                                />
                                <PrimeVueButton
                                    icon="pi pi-times"
                                    class="p-button-rounded p-button-sm p-button-danger p-button-outlined remove-btn"
                                    @click="removeUploadImage"
                                    v-tooltip.top="'Remove this image'"
                                />
                            </template>

                            <div class="empty-image flex flex-center" v-else>
                                <p>👈 Upload A Photo</p>
                            </div>
                        </div>
                    </div>

                    <!-- Submiitting button -->
                    <div class="field col-12 flex">
                        <PrimeVueButton
                            type="button"
                            label="Save"
                            icon="pi pi-save"
                            class="submit-btn p-button-success mb-2 mr-2"
                            @click="submitData"
                            :loading="loading"
                        />
                        <PrimeVueButton
                            type="button"
                            label="Delete"
                            icon="pi pi-trash"
                            class="delete-btn mb-2 mr-2"
                            @click="openConfirm"
                            :loading="loading"
                            v-if="isEditPage"
                        />
                    </div>
                </div>
            </div>
        </div>
    </div>

    <!-- Confirm message -->
    <Toast position="bottom-center" group="event-confirm" v-if="isEditPage">
        <template #message="slotProps">
            <div class="flex flex-column toast-container">
                <div class="text-center toast-content">
                    <i
                        class="pi pi-exclamation-triangle"
                        style="font-size: 3rem"
                    ></i>
                    <p>Are you sure to delete this event?</p>
                </div>
                <div class="grid p-fluid">
                    <div class="col-6">
                        <PrimeVueButton
                            label="Yes"
                            @click="deleteEvent"
                        ></PrimeVueButton>
                    </div>
                    <div class="col-6">
                        <PrimeVueButton
                            class="p-button-secondary"
                            label="No"
                            @click="toast.removeGroup('event-confirm')"
                        ></PrimeVueButton>
                    </div>
                </div>
            </div>
        </template>
    </Toast>
</template>

<style lang="scss" scoped>
.title {
    font-weight: 900;
    color: var(--primary-color);
    text-align: center;
    margin-bottom: 2rem;
}

.image-wrapper {
    padding: 2rem 0.5rem !important;
    border: 1px solid lightgray;
    border-radius: 15px;
    position: relative;

    .image-preview {
        width: 80%;
        border-radius: 15px;
    }

    .empty-image {
        width: 60%;
        aspect-ratio: 1 / 1;
        border-radius: 15px;
        border: 2px dashed lightgray;

        p {
            font-weight: 700;
            color: lightgray;
        }
    }

    .remove-btn {
        position: absolute;
        top: calc(2rem);
        right: 5px;
        background-color: #fff;
    }
}

.cancel-btn,
.delete-btn,
.submit-btn {
    margin-top: 3em;
    width: 8em;
}
.delete-btn {
    background: rgb(246, 76, 76);
}
</style>
