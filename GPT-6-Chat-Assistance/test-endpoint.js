const url6 = "https://molabantirupak-5013-resource.services.ai.azure.com/openai/v1/responses";
const url5 = "https://molabantirupak-5013-resource.services.ai.azure.com/api/projects/molabantirupak-5013/openai/v1/responses";
const apiKey = process.env.AZURE_OPENAI_API_KEY;

async function test(url, modelName) {
  const payload = { input: [{ role: "user", content: "Hello" }], max_tokens: 100 };
  if (modelName === "gpt-5-mini") {
    payload.model = "gpt-5-mini";
    payload.input = "Hello"; // Assuming string input for gpt-5-mini
  }
  const res = await fetch(url, {
    method: "POST",
    headers: { "Content-Type": "application/json", "api-key": apiKey },
    body: JSON.stringify(payload),
  });
  console.log("Status: ", res.status);
}

test(url6, "gpt-6-astra");
test(url5, "gpt-5-mini");
