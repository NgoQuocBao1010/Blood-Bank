<script setup>
import { ref } from "vue";

const activeId = ref(0);

const items = ref([
    { id: 1, scrollId: "#why", label: "Why?" },
    { id: 2, scrollId: "#benefits", label: "Benefits" },
    { id: 3, scrollId: "#facts", label: "Facts" },
    { id: 4, scrollId: "#before", label: "Before donate" },
    { id: 5, scrollId: "#pre-donate", label: "Pre-donate" },
    { id: 6, scrollId: "#donate", label: "Donate" },
    { id: 7, scrollId: "#post-donate", label: "Post-donate" },
]);

const smoothScroll = (scrollId) => {
    document.querySelector(scrollId).scrollIntoView({
        behavior: "smooth",
    });
};

const handleClick = (itemId) => {
    activeId.value = itemId;
};
</script>

<template>
    <div>
        <ul
            class="list-none p-0 m-0 flex lg:align-items-center select-none lg:flex-column cursor-pointer sidebar-scroll-intoview"
        >
            <li
                v-for="item in items"
                :key="item.id"
                @click="smoothScroll(item.scrollId)"
            >
                <a
                    class="flex m-0 ml-3 px-0 py-3 text-900 font-medium line-height-3 p-ripple justify-content-center"
                    :class="{ active: item.id === activeId }"
                    @click="() => handleClick(item.id)"
                >
                    <span>{{ item.label }}</span>
                </a>
            </li>
        </ul>
        <div class="py-4 px-4 lg:px-8 mx-0 lg:mx-8" id="highlight">
            <div class="text-center">
                <h1 class="text-900 font-normal mb-2 text-4xl">
                    Did you KNOW?
                </h1>
                <span class="text-600 text-xl"><i>Blood Bank</i></span>
            </div>

            <div class="grid mt-8 pb-2 md:pb-8" id="why">
                <div
                    class="flex justify-content-center col-12 lg:col-6 p-0 flex-order-1 lg:flex-order-0"
                    style="
                        border-radius: 8px;
                        background-color: var(--PRIMARY_COLOR);
                    "
                >
                    <img
                        src="../../assets/images/about-img-1.png"
                        class="w-9"
                        alt="mockup mobile"
                    />
                </div>

                <div
                    class="col-12 lg:col-6 my-auto flex flex-column lg:align-items-end lg:text-right align-items-center text-center"
                >
                    <div
                        class="flex align-items-center justify-content-center"
                        style="
                            width: 3.2rem;
                            height: 3.2rem;
                            border-radius: 10px;
                            background-color: var(--PRIMARY_COLOR);
                        "
                    >
                        <i
                            class="pi pi-fw pi-question-circle text-3xl text-white-700"
                            style="background-color: var(--PRIMARY_COLOR)"
                        ></i>
                    </div>
                    <h2 class="line-height-3 text-900 text-2xl font-normal">
                        Why Donate Blood
                    </h2>
                    <span
                        class="text-700 text-xl line-height-2 ml-0 md:ml-2"
                        style="max-width: 650px"
                    >
                        <p>
                            The most precious gift that one can give to another
                            person is the gift of life i.e. blood. It is the
                            essence of life.
                        </p>

                        <p>
                            Your blood saves more than one life when it is
                            separated into its components (red blood cells,
                            plasma etc.).
                        </p>
                        <p>
                            Blood is needed regularly for patients with diseases
                            such as thalassemia and hemophilia, and also for the
                            treatment of injuries after an accident, major
                            surgeries, anemia, etc.
                        </p>
                        <p>It improves your health.</p>
                    </span>
                </div>
            </div>

            <div class="grid mt-8 pt-2 md:pb-8" id="benefits">
                <div
                    class="col-12 lg:col-6 my-auto flex flex-column lg:align-items-start align-items-center"
                >
                    <div
                        class="flex align-items-center justify-content-center"
                        style="
                            width: 3.2rem;
                            height: 3.2rem;
                            border-radius: 10px;
                            background-color: var(--PRIMARY_COLOR);
                        "
                    >
                        <i
                            class="pi pi-fw pi-heart text-3xl"
                            style="background-color: var(--PRIMARY_COLOR)"
                        ></i>
                    </div>
                    <h2 class="line-height-3 text-900 text-2xl font-normal">
                        Benefits of Donating Blood
                    </h2>
                    <span
                        class="text-700 text-xl line-height-2 mr-0 md:mr-2"
                        style="max-width: 650px"
                    >
                        <h4>Less likely to suffer diseases</h4>
                        <p>
                            Did you know that people who donate blood are 88%
                            less likely to suffer a heart attack and 33% less
                            likely to acquire any type of cardiovascular
                            disease.
                        </p>
                        <p>
                            When you donate blood, it removes 225 to 250
                            milligrams of iron from your body, hence reducing
                            the risk of heart disease.
                        </p>
                        <p>
                            Blood Center performs numerous tests on the donated
                            blood. Therefore regular blood donation helps in
                            sheilding you from serious diseases.
                        </p>
                    </span>
                </div>

                <div
                    class="flex justify-content-end flex-order-1 sm:flex-order-2 col-12 lg:col-6 p-0"
                    style="
                        border-radius: 8px;
                        background-color: var(--PRIMARY_COLOR);
                    "
                >
                    <img
                        src="../../assets/images/about-img-2.jpeg"
                        class="w-11 pt-4"
                        alt="mockup"
                    />
                </div>
            </div>

            <div class="grid mt-8 pb-2 md:pb-8" id="facts">
                <div
                    class="flex justify-content-center col-12 lg:col-6 p-0 flex-order-1 lg:flex-order-0"
                    style="
                        border-radius: 8px;
                        background-color: var(--PRIMARY_COLOR);
                    "
                >
                    <img
                        src="../../assets/images/about-img-3.jpeg"
                        class="w-9"
                        alt="mockup mobile"
                    />
                </div>

                <div
                    class="col-12 lg:col-6 my-auto flex flex-column lg:align-items-end lg:text-right align-items-center text-center"
                >
                    <div
                        class="flex align-items-center justify-content-center"
                        style="
                            width: 3.2rem;
                            height: 3.2rem;
                            border-radius: 10px;
                            background-color: var(--PRIMARY_COLOR);
                        "
                    >
                        <i
                            class="pi pi-fw pi-star-fill text-3xl text-white-700"
                            style="background-color: var(--PRIMARY_COLOR)"
                        ></i>
                    </div>
                    <h2 class="line-height-3 text-900 text-2xl font-normal">
                        Facts About Blood
                    </h2>
                    <span
                        class="text-700 text-xl line-height-2 ml-0 md:ml-2"
                        style="max-width: 650px"
                    >
                        <p>
                            One unit of donated blood can save up to three
                            lives.
                        </p>

                        <p>
                            You can donate blood every three months. It only
                            takes 48 hours for your body fluids to be completely
                            replenished.
                        </p>
                        <p>
                            Scientists have estimated the volume of blood in the
                            human body to be eight percent of body weight
                        </p>
                        <p>
                            There are 100,000 miles of blood vessels in an adult
                            human body.
                        </p>
                        <p>
                            A red blood cell can make a complete circuit of your
                            body in 30 seconds.
                        </p>
                        <p>White blood cells make up about 1% of your blood.</p>
                    </span>
                </div>
            </div>

            <div class="grid mt-8 pt-2 md:pb-8" id="before">
                <div
                    class="col-12 lg:col-6 my-auto flex flex-column lg:align-items-start align-items-center"
                >
                    <div
                        class="flex align-items-center justify-content-center"
                        style="
                            width: 3.2rem;
                            height: 3.2rem;
                            border-radius: 10px;
                            background-color: var(--PRIMARY_COLOR);
                        "
                    >
                        <i
                            class="pi pi-fw pi-thumbs-up text-3xl"
                            style="background-color: var(--PRIMARY_COLOR)"
                        ></i>
                    </div>
                    <h2 class="line-height-3 text-900 text-2xl font-normal">
                        Before You Donate
                    </h2>
                    <span
                        class="text-700 text-xl line-height-2 mr-0 md:mr-2"
                        style="max-width: 650px"
                    >
                        <p>
                            Donating blood is a safe, simple, and rewarding
                            experience that usually takes 30 minutes.
                        </p>
                        <p>
                            To donate blood, find a blood bank near you using
                            <span style="color: var(--DARK_PINK)"
                                ><b>Blood Bank Search</b></span
                            >.
                        </p>
                        <p>
                            Requirement before Blood Donation is that your
                            weight should be atleast 45 kgs , be at least 18
                            years old and be healthy in general.
                        </p>
                        <p>
                            If you have any particular health concerns then
                            inform the blood bank at the time of blood donation.
                        </p>
                    </span>
                </div>

                <div
                    class="flex justify-content-end flex-order-1 sm:flex-order-2 col-12 lg:col-6 p-0"
                    style="
                        border-radius: 8px;
                        background-color: var(--PRIMARY_COLOR);
                    "
                >
                    <img
                        src="../../assets/images/about-img-4.jpeg"
                        class="w-11 pt-4"
                        alt="mockup"
                    />
                </div>
            </div>

            <div class="grid mt-8 pb-2 md:pb-8" id="pre-donate">
                <div
                    class="flex justify-content-center col-12 lg:col-6 p-0 flex-order-1 lg:flex-order-0"
                    style="
                        border-radius: 8px;
                        background-color: var(--PRIMARY_COLOR);
                    "
                >
                    <img
                        src="../../assets/images/about-img-5.jpeg"
                        class="w-9"
                        alt="mockup mobile"
                    />
                </div>

                <div
                    class="col-12 lg:col-6 my-auto flex flex-column lg:align-items-end lg:text-right align-items-center text-center"
                >
                    <div
                        class="flex align-items-center justify-content-center"
                        style="
                            width: 3.2rem;
                            height: 3.2rem;
                            border-radius: 10px;
                            background-color: var(--PRIMARY_COLOR);
                        "
                    >
                        <i
                            class="pi pi-fw pi-pencil text-3xl text-white-700"
                            style="background-color: var(--PRIMARY_COLOR)"
                        ></i>
                    </div>
                    <h2 class="line-height-3 text-900 text-2xl font-normal">
                        Pre Donation Screening
                    </h2>
                    <span
                        class="text-700 text-xl line-height-2 ml-0 md:ml-2"
                        style="max-width: 650px"
                    >
                        <p>
                            During pre-donation screening, a blood bank employee
                            will ask you some questions about your health,
                            lifestyle, and disease risk factors. All of this
                            information is confidential.
                        </p>

                        <p>
                            An employee will perform a short health exam, taking
                            your pulse, temperature and blood pressure. A drop
                            of blood from your finger will also be tested to
                            ensure that your blood iron level is sufficient for
                            you to donate.
                        </p>
                        <p>
                            All medical equipment used for this test, as well as
                            during the donation process, is sterile, used only
                            once and then disposed.
                        </p>
                    </span>
                </div>
            </div>

            <div class="grid mt-8 pt-2 md:pb-8" id="donate">
                <div
                    class="col-12 lg:col-6 my-auto flex flex-column lg:align-items-start align-items-center"
                >
                    <div
                        class="flex align-items-center justify-content-center"
                        style="
                            width: 3.2rem;
                            height: 3.2rem;
                            border-radius: 10px;
                            background-color: var(--PRIMARY_COLOR);
                        "
                    >
                        <i
                            class="pi pi-fw pi-plus-circle text-3xl"
                            style="background-color: var(--PRIMARY_COLOR)"
                        ></i>
                    </div>
                    <h2 class="line-height-3 text-900 text-2xl font-normal">
                        Blood Donation
                    </h2>
                    <span
                        class="text-700 text-xl line-height-2 mr-0 md:mr-2"
                        style="max-width: 650px"
                    >
                        <p>
                            Once the pre-donation screening is finished, you
                            will proceed to a donor bed where your arm will be
                            cleaned with an antiseptic, and a professional will
                            use a blood donation kit to draw blood from a vein
                            in your arm.
                        </p>
                        <p>
                            If you are allergic to iodine, be sure to tell the
                            phlebotomist at this point.
                        </p>
                        <p>
                            During the donation process, you will donate one
                            unit of blood; this takes about six to ten minutes.
                        </p>
                    </span>
                </div>

                <div
                    class="flex justify-content-end flex-order-1 sm:flex-order-2 col-12 lg:col-6 p-0"
                    style="
                        border-radius: 8px;
                        background-color: var(--PRIMARY_COLOR);
                    "
                >
                    <img
                        src="../../assets/images/about-img-6.jpeg"
                        class="w-11 pt-4"
                        alt="mockup"
                    />
                </div>
            </div>

            <div class="grid mt-8 pb-2 md:pb-8" id="post-donate">
                <div
                    class="flex justify-content-center col-12 lg:col-6 p-0 flex-order-1 lg:flex-order-0"
                    style="
                        border-radius: 8px;
                        background-color: var(--PRIMARY_COLOR);
                    "
                >
                    <img
                        src="../../assets/images/about-img-7.jpeg"
                        class="w-9"
                        alt="mockup mobile"
                    />
                </div>

                <div
                    class="col-12 lg:col-6 my-auto flex flex-column lg:align-items-end lg:text-right align-items-center text-center"
                >
                    <div
                        class="flex align-items-center justify-content-center"
                        style="
                            width: 3.2rem;
                            height: 3.2rem;
                            border-radius: 10px;
                            background-color: var(--PRIMARY_COLOR);
                        "
                    >
                        <i
                            class="pi pi-fw pi-eye text-3xl text-white-700"
                            style="background-color: var(--PRIMARY_COLOR)"
                        ></i>
                    </div>
                    <h2 class="line-height-3 text-900 text-2xl font-normal">
                        Post Donation
                    </h2>
                    <span
                        class="text-700 text-xl line-height-2 ml-0 md:ml-2"
                        style="max-width: 650px"
                    >
                        <p>
                            Following your donation, you will receive
                            refreshments.
                        </p>

                        <p>
                            After donating, it is recommended that you increase
                            your fluid intake for the next 24 to 48 hours; avoid
                            strenuous physical exertion, heavy lifting or
                            pulling with the donation arm for about five hours;
                            and eat well balanced meals for the next 24 hours.
                        </p>
                        <p>
                            Smoking and alcohol consumption is not recommended
                            after donation. Although donors seldom experience
                            discomfort after donating, if you feel light-headed,
                            lie down until the feeling passes.
                        </p>
                    </span>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
ul.sidebar-scroll-intoview {
    position: fixed !important;
    left: 0;

    li {
        width: 100%;
        a:hover {
            border-radius: 10px;
            width: 100%;
            background-color: var(--PRIMARY_COLOR);
        }

        a.active {
            border-radius: 10px;
            width: 100%;
            background-color: var(--PRIMARY_COLOR);
        }
    }
}
</style>
