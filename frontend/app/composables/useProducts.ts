import type { CreateProductPayload, Product } from "~/types";
import { parseApiError } from "~/utils/parserApiError";

interface UseProducts {
  products: Ref<Product[]>;
  loading: Ref<boolean>;
  error: Ref<string | null>;
  fetchProducts: (categoryId?: number) => Promise<void>;
  createProduct: (payload: CreateProductPayload) => Promise<Product | null>;
  updateProduct: (
    id: number,
    payload: CreateProductPayload,
  ) => Promise<boolean>;
  deleteProduct: (id: number) => Promise<boolean>;
  clearError: () => void;
}

export const useProducts = (): UseProducts => {
  const config = useRuntimeConfig();
  const base = config.public.apiBase;

  const products = ref<Product[]>([]);
  const loading = ref(false);
  const error = ref<string | null>(null);

  const clearError = (): void => {
    error.value = null;
  };

  const fetchProducts = async (categoryId?: number): Promise<void> => {
    loading.value = true;
    clearError();
    try {
      const url = categoryId
        ? `${base}/api/products?categoryId=${categoryId}`
        : `${base}/api/products`;
      products.value = await $fetch<Product[]>(url);
    } catch (err: unknown) {
      error.value = parseApiError(err, "Erro ao carregar os produtos");
    } finally {
      loading.value = false;
    }
  };

  const createProduct = async (
    payload: CreateProductPayload,
  ): Promise<Product | null> => {
    loading.value = true;
    clearError();
    try {
      const newProduct = await $fetch<Product>(`${base}/api/products`, {
        method: "post",
        body: payload,
      });
      products.value.push(newProduct);
      return newProduct;
    } catch (err: unknown) {
      error.value = parseApiError(err, "Erro ao criar produto.");
      return null;
    } finally {
      loading.value = false;
    }
  };

  const updateProduct = async (
    id: number,
    payload: CreateProductPayload,
  ): Promise<boolean> => {
    loading.value = true;
    clearError();
    try {
      const updatedProduct = await $fetch<Product>(
        `${base}/api/products/${id}`,
        {
          method: "PUT",
          body: payload,
        },
      );
      const idx = products.value.findIndex((p) => p.id === id);
      if (idx !== -1) products.value[idx] = updatedProduct;
      return true;
    } catch (err: unknown) {
      error.value = parseApiError(err, "Erro ao atualizar produto.");
      return false;
    } finally {
      loading.value = false;
    }
  };

  const deleteProduct = async (id: number): Promise<boolean> => {
    loading.value = true;
    clearError();
    try {
      await $fetch(`${base}/api/products/${id}`, { method: "DELETE" });
      products.value = products.value.filter((p) => p.id !== id);
      return true;
    } catch (err: unknown) {
      error.value = parseApiError(err, "Erro ao excluir produto.");
      return false;
    } finally {
      loading.value = false;
    }
  };

  return {
    products,
    loading,
    error,
    fetchProducts,
    createProduct,
    updateProduct,
    deleteProduct,
    clearError,
  };
};
