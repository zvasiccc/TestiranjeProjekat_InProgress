import { test, expect } from "@playwright/test";

test("should display home page", async ({ page }) => {
  await page.goto("/");
  const title = await page.title();
  expect(title).toBe("RWAFrontend");
});
