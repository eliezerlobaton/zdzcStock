import type { Category, CreateCategoryPayload } from "~/types";
import { parseApiError } from "~/utils/parserApiError";

interface UseCategories {
  categories: Ref<Category[]>;
  loading: Ref<boolean>;
  error: Ref<string | null>;
  fetchCategories: () => Promise<void>;
  createCategory: (payload: CreateCategoryPayload) => Promise<Category | null>;
  updateCategory: (
    id: number,
    payload: CreateCategoryPayload,
  ) => Promise<boolean>;
  deleteCategory: (id: number) => Promise<boolean>;
  clearError: () => void;
}

export const useCategories = (): UseCategories => {
  const config = useRuntimeConfig();
  const base = config.public.apiBase;

  const categories = ref<Category[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);

  const clearError = (): void => {
    error.value = null;
  };

  const fetchCategories = async (): Promise<void> => {
    loading.value = true;
    clearError();
    try {
      categories.value = await $fetch<Category[]>(`${base}/api/categories`);
    } catch (err: unknown) {
      error.value = parseApiError(err, "Erro ao carregar categorias.");
    } finally {
      loading.value = false;
    }
  };

  const createCategory = async (
    payload: CreateCategoryPayload,
  ): Promise<Category | null> => {
    loading.value = true;
    clearError();
    try {
      const newCategory = await $fetch<Category>(`${base}/api/categories`, {
        method: "POST",
        body: payload,
      });
      categories.value.push(newCategory);
      return newCategory;
    } catch (err: unknown) {
      error.value = parseApiError(err, "Erro ao criar categoria");
      return null;
    } finally {
      loading.value = false;
    }
  };

  const updateCategory = async (
    id: number,
    payload: CreateCategoryPayload,
  ): Promise<boolean> => {
    loading.value = true;
    clearError();
    try {
      const updatedCategory = await $fetch<Category>(
        `${base}/api/categories/${id}`,
        {
          method: "PUT",
          body: payload,
        },
      );

      const idx = categories.value.findIndex((c) => c.id === id);
      if (idx !== -1) categories.value[idx] = updatedCategory;
      return true;
    } catch (err: unknown) {
      error.value = parseApiError(err, "Erro ao atualizar categoria");
      return false;
    } finally {
      loading.value = false;
    }
  };

  const deleteCategory = async (id: number): Promise<boolean> => {
    loading.value = true;
    clearError();
    try {
      await $fetch(`${base}/api/categories/${id}`, { method: "DELETE" });

      categories.value = categories.value.filter((c) => c.id !== id);
      return true;
    } catch (err: unknown) {
      error.value = parseApiError(err, "Erro ao excluir categoria");
      return false;
    } finally {
      loading.value = false;
    }
  };

  return {
    categories,
    loading,
    error,
    fetchCategories,
    createCategory,
    updateCategory,
    deleteCategory,
    clearError,
  };
};
