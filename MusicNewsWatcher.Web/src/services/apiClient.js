import { Configuration, DownloadHistoryApi } from "../api/client.ts";

/**
 * @typedef {import("../api/client.ts/models/TrackDownloadHistoryDto").TrackDownloadHistoryDto} TrackDownloadHistoryDto
 */

// Base configuration for API (can be extended per-request)
export const baseConfig = new Configuration({ basePath: "http://localhost:8050" });

export function getCookie(name) {
  if (typeof document === "undefined") return "";
  return document.cookie.split("; ").reduce((r, v) => {
    const parts = v.split("=");
    return parts[0] === name ? decodeURIComponent(parts.slice(1).join("=")) : r;
  }, "");
}

export function setCookie(name, value, days = 365) {
  if (typeof document === "undefined") return;
  const expires = new Date(Date.now() + days * 864e5).toUTCString();
  document.cookie = `${name}=${encodeURIComponent(value)}; expires=${expires}; path=/`;
}

/**
 * Create API client configured with Authorization header
 * @param {string} token
 * @returns {DownloadHistoryApi}
 */
export function createApiWithToken(token) {
  const cfg = new Configuration({ basePath: baseConfig.basePath, headers: { Authorization: `Bearer ${token}` } });
  return new DownloadHistoryApi(cfg);
}

/**
 * Fetch download history from API
 * @param {number} [limit=50]
 * @param {string} token
 * @returns {Promise<TrackDownloadHistoryDto[]>}
 */
export async function fetchDownloadHistory(limit = 50, token) {
  const api = createApiWithToken(token);
    const res = await api.apiDownloadHistoryGet({ limit });
  return Array.isArray(res) ? res : [];
}
