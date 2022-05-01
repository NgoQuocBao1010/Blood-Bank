<script setup></script>

<template>
    <div class="grid">
        <div class="col-12">
            <div class="card">
                <!-- logo -->
                <div class="side">
                    <img
                        src="../assets/images/undraw_engineering_team_u99p.png"
                        alt="Team Image"
                        class="team-img"
                    />
                </div>

                <!-- Introduction -->
                <div class="side">
                    <p class="title">About Us</p>

                    <ul class="information-items">
                        <li class="item">
                            <p class="subtitle">
                                <i class="fa-solid fa-signature"></i>
                                Team
                            </p>
                            <p class="content">Best Friends</p>
                        </li>

                        <li class="item">
                            <p class="subtitle">
                                <i class="fa-brands fa-github"></i>
                                Github
                            </p>
                            <a
                                href="https://github.com/NgoQuocBao1010/Blood-Bank"
                                class="content"
                                target="_blank"
                            >
                                Blood Bank Project
                            </a>
                        </li>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</template>

<style lang="scss" scoped>
.card {
    display: flex;
    align-items: center;

    .side {
        img.team-img {
            width: 30vw;
        }

        p {
            padding: 0;
            margin: 0;
        }

        p.title {
            text-transform: uppercase;
            font-size: 2rem;
            font-weight: 900;
            color: var(--primary-color);
            margin-bottom: 2rem;
        }

        ul.information-items {
            padding: 0;
            margin: 0;

            li.item {
                list-style: none;
                display: flex;
                align-items: center;
                margin-bottom: 1.2rem;

                p.subtitle {
                    width: 6em;
                    padding: 0.25rem;
                    margin-right: 20px;
                    border-radius: 10px;
                    box-shadow: rgba(0, 0, 0, 0.15) 1.95px 1.95px 2.6px;
                    text-align: center;
                }

                .content {
                    font-weight: 500;
                    font-size: 1.2rem;
                }
            }
        }
    }
}
</style>
