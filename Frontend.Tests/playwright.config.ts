import { defineConfig } from "@playwright/test";

export default defineConfig({
  testDir: "./tests", // direktorijum gde ce se nalaziti testovi
  timeout: 30000, // timeout za svaki test
  use: {
    baseURL: "http://localhost:4200", // promenite na URL vaše aplikacije
    headless: true, // postavite na false za debug
    viewport: { width: 1280, height: 720 },
    video: "retain-on-failure", // čuva video ako test ne uspe
    screenshot: "only-on-failure", // čuva screenshot ako test ne uspe
  },
});
